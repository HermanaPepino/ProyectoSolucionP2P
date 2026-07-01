using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OperacionService : IOperacionService
    {
        private const int DuracionTemporizadorMinutos = 15;

        private readonly IOperacionRepository _repo;
        private readonly IOfertaRepository _ofertaRepo;
        private readonly ITemporizadorOperacionRepository _temporizadorRepo;
        private readonly INotificacionService _notificaciones;

        public OperacionService(
            IOperacionRepository repo,
            IOfertaRepository ofertaRepo,
            ITemporizadorOperacionRepository temporizadorRepo,
            INotificacionService notificaciones)
        {
            _repo = repo;
            _ofertaRepo = ofertaRepo;
            _temporizadorRepo = temporizadorRepo;
            _notificaciones = notificaciones;
        }

        public async Task<IEnumerable<OperacionDto>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            var resultado = new List<OperacionDto>();
            foreach (var e in lista)
                resultado.Add(await EnriquecerYExpirarAsync(e));
            return resultado;
        }

        public async Task<OperacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : await EnriquecerYExpirarAsync(e);
        }

        public async Task<OperacionDto> CreateAsync(OperacionDto dto)
        {
            var e = new Operacion
            {
                OfertaId = dto.OfertaId,
                CompradorId = dto.CompradorId,
                VendedorId = dto.VendedorId,
                Monto = dto.Monto,
                Estado = dto.Estado,
                CodigoOperacion = dto.CodigoOperacion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                FechaLiberacion = dto.FechaLiberacion
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, OperacionDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OfertaId = dto.OfertaId;
            e.CompradorId = dto.CompradorId;
            e.VendedorId = dto.VendedorId;
            e.Monto = dto.Monto;
            e.Estado = dto.Estado;
            e.CodigoOperacion = dto.CodigoOperacion;
            e.FechaInicio = dto.FechaInicio;
            e.FechaFin = dto.FechaFin;
            e.FechaLiberacion = dto.FechaLiberacion;
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

        // ---------------------------------------------------------------
        // HU-008: Iniciar Trato P2P  /  HU-009: Activar Temporizador
        // ---------------------------------------------------------------
        public async Task<(OperacionDto? operacion, string? error)> IniciarTratoAsync(IniciarTratoDto dto, int compradorId)
        {
            var oferta = await _ofertaRepo.GetByIdAsync(dto.OfertaId);
            if (oferta == null)
                return (null, "La oferta no existe.");

            if (oferta.Estado != "Activa")
                return (null, "Esta oferta ya no está disponible (fue reservada o cerrada).");

            if (oferta.UsuarioId == compradorId)
                return (null, "No puedes iniciar un trato sobre tu propia oferta.");

            if (dto.Monto < oferta.MontoMinimo || dto.Monto > oferta.MontoMaximo)
                return (null, $"El monto debe estar entre {oferta.MontoMinimo} y {oferta.MontoMaximo}.");

            var vendedorId = oferta.UsuarioId;
            var ahora = DateTime.Now;

            var nuevaOperacion = new Operacion
            {
                OfertaId = oferta.Id,
                CompradorId = compradorId,
                VendedorId = vendedorId,
                Monto = dto.Monto,
                Estado = "En proceso",
                CodigoOperacion = GenerarCodigoOperacion(),
                FechaInicio = ahora
            };

            var creada = await _repo.CreateAsync(nuevaOperacion);

            // Bloquea temporalmente la oferta
            oferta.Estado = "Reservada";
            await _ofertaRepo.UpdateAsync(oferta);

            // HU-009: arranca el temporizador de seguridad (15 minutos)
            var temporizador = new TemporizadorOperacion
            {
                OperacionId = creada.Id,
                FechaInicio = ahora,
                FechaFin = ahora.AddMinutes(DuracionTemporizadorMinutos),
                Estado = "Activo"
            };
            await _temporizadorRepo.CreateAsync(temporizador);

            // Notifica a ambas partes
            await NotificarAsync(compradorId, "Trato iniciado",
                $"Iniciaste un trato (código {nuevaOperacion.CodigoOperacion}). Tienes {DuracionTemporizadorMinutos} minutos para pagar.", creada.Id);
            await NotificarAsync(vendedorId, "Nuevo trato sobre tu oferta",
                $"Un comprador inició un trato sobre tu oferta (código {nuevaOperacion.CodigoOperacion}).", creada.Id);

            var completa = await _repo.GetByIdAsync(creada.Id);
            return (await EnriquecerYExpirarAsync(completa!), null);
        }

        public async Task<(bool ok, string? error)> CancelarAsync(int operacionId, int usuarioId)
        {
            var operacion = await _repo.GetByIdAsync(operacionId);
            if (operacion == null) return (false, "La operación no existe.");

            if (operacion.CompradorId != usuarioId && operacion.VendedorId != usuarioId)
                return (false, "No tienes permiso sobre esta operación.");

            if (operacion.Estado != "En proceso")
                return (false, "Solo se pueden cancelar operaciones en proceso.");

            operacion.Estado = "Cancelada";
            await _repo.UpdateAsync(operacion);

            await ExpirarTemporizadorAsync(operacionId, "Cancelado");
            await LiberarOfertaAsync(operacion.OfertaId);

            var contraparte = usuarioId == operacion.CompradorId ? operacion.VendedorId : operacion.CompradorId;
            await NotificarAsync(contraparte, "Operación cancelada",
                $"La operación {operacion.CodigoOperacion} fue cancelada.", operacionId);

            return (true, null);
        }

        // ---------------------------------------------------------------
        // HU-010 / HU-011: el comprador marca que ya pagó
        // ---------------------------------------------------------------
        public async Task<(bool ok, string? error)> MarcarPagoEnviadoAsync(int operacionId, int usuarioId)
        {
            var operacion = await _repo.GetByIdAsync(operacionId);
            if (operacion == null) return (false, "La operación no existe.");

            if (operacion.CompradorId != usuarioId)
                return (false, "Solo el comprador puede marcar el pago como enviado.");

            if (operacion.Estado != "En proceso")
                return (false, "La operación ya no admite esta acción.");

            operacion.Estado = "Pago enviado";
            await _repo.UpdateAsync(operacion);

            await NotificarAsync(operacion.VendedorId, "Comprobante recibido",
                $"El comprador subió el comprobante de pago de la operación {operacion.CodigoOperacion}. Verifícalo y confirma la recepción.",
                operacionId);

            return (true, null);
        }

        // ---------------------------------------------------------------
        // HU-012: el vendedor confirma que recibió el pago -> libera la operación
        // ---------------------------------------------------------------
        public async Task<(bool ok, string? error)> ConfirmarRecepcionPagoAsync(int operacionId, int usuarioId)
        {
            var operacion = await _repo.GetByIdAsync(operacionId);
            if (operacion == null) return (false, "La operación no existe.");

            if (operacion.VendedorId != usuarioId)
                return (false, "Solo el vendedor puede confirmar la recepción del pago.");

            if (operacion.Estado != "Pago enviado")
                return (false, "Aún no se ha subido el comprobante de pago.");

            var ahora = DateTime.Now;
            operacion.Estado = "Completada";
            operacion.FechaFin = ahora;
            operacion.FechaLiberacion = ahora;
            await _repo.UpdateAsync(operacion);

            await ExpirarTemporizadorAsync(operacionId, "Completado");

            // La oferta ya se concretó: se cierra.
            var oferta = await _ofertaRepo.GetByIdAsync(operacion.OfertaId);
            if (oferta != null && oferta.Estado == "Reservada")
            {
                oferta.Estado = "Cerrada";
                await _ofertaRepo.UpdateAsync(oferta);
            }

            await NotificarAsync(operacion.CompradorId, "Intercambio completado",
                $"El vendedor confirmó la recepción del pago. La operación {operacion.CodigoOperacion} se completó con éxito. ¡Califica tu experiencia!",
                operacionId);

            return (true, null);
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------

        private async Task<OperacionDto> EnriquecerYExpirarAsync(Operacion e)
        {
            var dto = MapToDto(e);

            var temporizador = await _temporizadorRepo.GetByOperacionIdAsync(e.Id);
            if (temporizador == null) return dto;

            var ahora = DateTime.Now;

            if (temporizador.Estado == "Activo" && ahora >= temporizador.FechaFin)
            {
                if (e.Estado == "En proceso")
                {
                    e.Estado = "Expirada";
                    await _repo.UpdateAsync(e);
                    await LiberarOfertaAsync(e.OfertaId);

                    await NotificarAsync(e.CompradorId, "Operación expirada",
                        $"El tiempo para pagar la operación {e.CodigoOperacion} expiró.", e.Id);
                    await NotificarAsync(e.VendedorId, "Operación expirada",
                        $"La operación {e.CodigoOperacion} expiró sin pago. Tu oferta vuelve a estar activa.", e.Id);
                }

                temporizador.Estado = "Expirado";
                await _temporizadorRepo.UpdateAsync(temporizador);

                dto.Estado = e.Estado;
            }

            dto.TemporizadorFechaFin = temporizador.FechaFin;
            dto.TemporizadorEstado = temporizador.Estado;
            dto.SegundosRestantes = temporizador.Estado == "Activo"
                ? Math.Max(0, (int)(temporizador.FechaFin - ahora).TotalSeconds)
                : 0;

            return dto;
        }

        private async Task ExpirarTemporizadorAsync(int operacionId, string nuevoEstado)
        {
            var temporizador = await _temporizadorRepo.GetByOperacionIdAsync(operacionId);
            if (temporizador != null && temporizador.Estado == "Activo")
            {
                temporizador.Estado = nuevoEstado;
                await _temporizadorRepo.UpdateAsync(temporizador);
            }
        }

        private async Task LiberarOfertaAsync(int ofertaId)
        {
            var oferta = await _ofertaRepo.GetByIdAsync(ofertaId);
            if (oferta != null && oferta.Estado == "Reservada")
            {
                oferta.Estado = "Activa";
                await _ofertaRepo.UpdateAsync(oferta);
            }
        }

        private async Task NotificarAsync(int usuarioId, string titulo, string mensaje, int? operacionId)
        {
            await _notificaciones.CreateAsync(new NotificacionDto
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensaje = mensaje,
                Leida = false,
                OperacionId = operacionId,
                FechaCreacion = DateTime.Now
            });
        }

        private static string GenerarCodigoOperacion()
            => "P2P-" + Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();

        private static OperacionDto MapToDto(Operacion e) => new OperacionDto
        {
            Id = e.Id,
            OfertaId = e.OfertaId,
            CompradorId = e.CompradorId,
            VendedorId = e.VendedorId,
            CompradorNombre = e.Comprador?.NombreCompleto ?? string.Empty,
            VendedorNombre = e.Vendedor?.NombreCompleto ?? string.Empty,
            Monto = e.Monto,
            Estado = e.Estado,
            CodigoOperacion = e.CodigoOperacion,
            FechaInicio = e.FechaInicio,
            FechaFin = e.FechaFin,
            FechaLiberacion = e.FechaLiberacion
        };
    }
}