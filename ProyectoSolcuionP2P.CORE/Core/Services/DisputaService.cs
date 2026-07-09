using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class DisputaService : IDisputaService
    {
        private const int PlazoDisputaDias = 30;

        private static readonly string[] EstadosActivosDisputa =
        {
            "En proceso",
            "Pago enviado"
        };

        private static readonly string[] EstadosFinalizadosDisputa =
        {
            "Completada",
            "Cancelada",
            "Expirada"
        };

        private readonly IDisputaRepository _repo;
        private readonly IOperacionRepository _operacionRepo;
        private readonly INotificacionService _notificaciones;

        public DisputaService(
            IDisputaRepository repo,
            IOperacionRepository operacionRepo,
            INotificacionService notificaciones)
        {
            _repo = repo;
            _operacionRepo = operacionRepo;
            _notificaciones = notificaciones;
        }

        public async Task<IEnumerable<DisputaDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<IEnumerable<DisputaHistorialDto>> GetAllHistorialAsync()
            => (await _repo.GetAllAsync()).Select(MapToHistorialDto);

        public async Task<DisputaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<DisputaDto> CreateAsync(DisputaDto dto)
        {
            var motivoLimpio = NormalizarMotivo(dto.Motivo);

            var e = new Disputa
            {
                OperacionId = dto.OperacionId,
                Motivo = motivoLimpio,
                Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Abierta" : dto.Estado.Trim(),
                Resolucion = string.IsNullOrWhiteSpace(dto.Resolucion) ? null : dto.Resolucion.Trim(),
                FechaRegistro = dto.FechaRegistro ?? DateTime.Now
            };

            var creado = await _repo.CreateAsync(e);
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, DisputaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            e.OperacionId = dto.OperacionId;
            e.Motivo = NormalizarMotivo(dto.Motivo);
            e.Estado = string.IsNullOrWhiteSpace(dto.Estado) ? e.Estado : dto.Estado.Trim();
            e.Resolucion = string.IsNullOrWhiteSpace(dto.Resolucion) ? null : dto.Resolucion.Trim();
            e.FechaRegistro = dto.FechaRegistro ?? e.FechaRegistro;

            await _repo.UpdateAsync(e);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        // HU-014: abre una disputa durante la operación o hasta 30 días después del cierre.
        // Regla de negocio: una sola disputa por operación. Si ya existe una disputa previa,
        // se debe continuar el seguimiento desde el historial, no crear otra.
        public async Task<(DisputaDto? disputa, string? error)> AbrirAsync(int operacionId, string motivo, int usuarioId)
        {
            if (operacionId <= 0)
                return (null, "La operación es obligatoria.");

            var motivoLimpio = NormalizarMotivo(motivo);

            if (motivoLimpio.Length < 20)
                return (null, "Describe el problema con al menos 20 caracteres.");

            var operacion = await _operacionRepo.GetByIdAsync(operacionId);
            if (operacion == null)
                return (null, "La operación no existe.");

            if (operacion.CompradorId != usuarioId && operacion.VendedorId != usuarioId)
                return (null, "No tienes permiso sobre esta operación.");

            var disputaExistente = await _repo.GetByOperacionIdAsync(operacionId);
            if (disputaExistente != null)
            {
                if (EsDisputaActiva(disputaExistente.Estado))
                    return (null, "Ya tienes una disputa en revisión para esta operación. Revisa el historial de disputas.");

                return (null, "Esta operación ya tuvo una disputa registrada. Revisa el historial de disputas.");
            }

            if (operacion.Estado == "En disputa")
                return (null, "Esta operación ya está en disputa. Espera la revisión de administración.");

            var estadoActivo = EstadosActivosDisputa.Contains(operacion.Estado);
            var estadoFinalizado = EstadosFinalizadosDisputa.Contains(operacion.Estado);

            if (!estadoActivo && !estadoFinalizado)
                return (null, "El estado actual de la operación no admite una nueva disputa.");

            if (estadoFinalizado)
            {
                var fechaBase = ObtenerFechaBaseParaPlazo(operacion);

                if (!fechaBase.HasValue)
                    return (null, "No se encontró una fecha válida para calcular el plazo de disputa.");

                var fechaLimite = fechaBase.Value.AddDays(PlazoDisputaDias);

                if (DateTime.Now > fechaLimite)
                    return (null, $"El plazo para abrir disputa venció. Solo puedes hacerlo hasta {PlazoDisputaDias} días calendario después del cierre.");
            }

            var disputa = new Disputa
            {
                OperacionId = operacionId,
                Motivo = motivoLimpio,
                Estado = "Abierta",
                FechaRegistro = DateTime.Now
            };

            var creada = await _repo.CreateAsync(disputa);

            if (estadoActivo)
            {
                // Si la operación aún estaba en curso, se congela para evitar que el flujo continúe.
                operacion.Estado = "En disputa";
                await _operacionRepo.UpdateAsync(operacion);
            }

            var contraparte = usuarioId == operacion.CompradorId ? operacion.VendedorId : operacion.CompradorId;

            var mensaje = estadoActivo
                ? $"Se abrió una disputa sobre la operación {operacion.CodigoOperacion}. La operación queda congelada hasta revisión."
                : $"Se abrió una disputa posterior al cierre sobre la operación {operacion.CodigoOperacion}. Administración revisará el caso.";

            await _notificaciones.CreateAsync(new NotificacionDto
            {
                UsuarioId = contraparte,
                Titulo = "Se abrió una disputa",
                Mensaje = mensaje,
                Leida = false,
                OperacionId = operacionId,
                FechaCreacion = DateTime.Now
            });

            return (MapToDto(creada), null);
        }


        // HU-015: administración resuelve la disputa y deja trazabilidad de la decisión.
        // Regla usada para el proyecto:
        // - A favor del vendedor: la operación queda Completada.
        // - A favor del comprador: la operación queda Cancelada.
        public async Task<(bool ok, string? error)> ResolverAsync(int id, string estadoResolucion, string resolucion)
        {
            var disputa = await _repo.GetByIdAsync(id);
            if (disputa == null)
                return (false, "La disputa no existe.");

            var estadoLimpio = (estadoResolucion ?? string.Empty).Trim();
            if (estadoLimpio != "Resuelta a favor comprador" && estadoLimpio != "Resuelta a favor vendedor")
                return (false, "Selecciona una resolución válida.");

            var resolucionLimpia = (resolucion ?? string.Empty).Trim();
            if (resolucionLimpia.Length < 10)
                return (false, "La resolución debe tener al menos 10 caracteres.");

            if (resolucionLimpia.Length > 1000)
                resolucionLimpia = resolucionLimpia[..1000];

            disputa.Estado = estadoLimpio;
            disputa.Resolucion = resolucionLimpia;

            await _repo.UpdateAsync(disputa);

            var operacion = await _operacionRepo.GetByIdAsync(disputa.OperacionId);
            if (operacion != null)
            {
                operacion.Estado = estadoLimpio == "Resuelta a favor vendedor" ? "Completada" : "Cancelada";
                operacion.FechaFin ??= DateTime.Now;
                operacion.FechaLiberacion ??= DateTime.Now;
                await _operacionRepo.UpdateAsync(operacion);

                await _notificaciones.CreateAsync(new NotificacionDto
                {
                    UsuarioId = operacion.CompradorId,
                    Titulo = "Disputa resuelta",
                    Mensaje = $"La disputa de la operación {operacion.CodigoOperacion} fue resuelta: {estadoLimpio}.",
                    Leida = false,
                    OperacionId = operacion.Id,
                    FechaCreacion = DateTime.Now
                });

                await _notificaciones.CreateAsync(new NotificacionDto
                {
                    UsuarioId = operacion.VendedorId,
                    Titulo = "Disputa resuelta",
                    Mensaje = $"La disputa de la operación {operacion.CodigoOperacion} fue resuelta: {estadoLimpio}.",
                    Leida = false,
                    OperacionId = operacion.Id,
                    FechaCreacion = DateTime.Now
                });
            }

            return (true, null);
        }

        public async Task<IEnumerable<DisputaHistorialDto>> GetMisDisputasAsync(int usuarioId)
            => (await _repo.GetByUsuarioIdAsync(usuarioId)).Select(MapToHistorialDto);

        public async Task<DisputaHistorialDto?> GetByOperacionForUserAsync(int operacionId, int usuarioId)
        {
            var disputa = await _repo.GetByOperacionIdAsync(operacionId);

            if (disputa == null) return null;

            if (disputa.Operacion.CompradorId != usuarioId && disputa.Operacion.VendedorId != usuarioId)
                return null;

            return MapToHistorialDto(disputa);
        }

        private static string NormalizarMotivo(string? motivo)
        {
            var limpio = motivo?.Trim() ?? string.Empty;
            return limpio.Length > 2000 ? limpio[..2000] : limpio;
        }

        private static bool EsDisputaActiva(string? estado)
        {
            var valor = (estado ?? string.Empty).Trim().ToLowerInvariant();
            return valor is "abierta" or "en revisión" or "en revision" or "pendiente";
        }

        private static DateTime? ObtenerFechaBaseParaPlazo(Operacion operacion)
        {
            // FechaFin es la referencia principal para operaciones completadas, canceladas o expiradas.
            // FechaLiberacion y FechaInicio quedan como respaldo para datos antiguos que no tengan FechaFin.
            return operacion.FechaFin ?? operacion.FechaLiberacion ?? operacion.FechaInicio;
        }

        private static DisputaDto MapToDto(Disputa e) => new DisputaDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            Motivo = e.Motivo,
            Estado = e.Estado,
            Resolucion = e.Resolucion,
            FechaRegistro = e.FechaRegistro
        };

        private static DisputaHistorialDto MapToHistorialDto(Disputa e)
        {
            var operacion = e.Operacion;

            return new DisputaHistorialDto
            {
                Id = e.Id,
                OperacionId = e.OperacionId,
                CodigoOperacion = operacion?.CodigoOperacion ?? string.Empty,
                EstadoOperacion = operacion?.Estado ?? string.Empty,
                Monto = operacion?.Monto ?? 0,
                CompradorId = operacion?.CompradorId ?? 0,
                VendedorId = operacion?.VendedorId ?? 0,
                CompradorNombre = operacion?.Comprador?.NombreCompleto ?? string.Empty,
                VendedorNombre = operacion?.Vendedor?.NombreCompleto ?? string.Empty,
                MetodoPagoNombre = operacion?.MetodoPago?.Nombre
                    ?? operacion?.OfertaMetodoPago?.MetodoPago?.Nombre
                    ?? string.Empty,
                Motivo = e.Motivo,
                Estado = e.Estado,
                Resolucion = e.Resolucion,
                FechaRegistro = e.FechaRegistro,
                FechaInicio = operacion?.FechaInicio,
                FechaFin = operacion?.FechaFin,
                FechaLiberacion = operacion?.FechaLiberacion,
                Evidencias = e.EvidenciaDisputa
                    .OrderByDescending(x => x.FechaSubida)
                    .Select(x => new EvidenciaDisputaDto
                    {
                        Id = x.Id,
                        DisputaId = x.DisputaId,
                        RutaArchivo = x.RutaArchivo,
                        FechaSubida = x.FechaSubida
                    })
                    .ToList()
            };
        }
    }
}
