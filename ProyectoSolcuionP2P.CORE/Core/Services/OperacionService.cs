using System.Text.Json;
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
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IUsuarioMetodoPagoRepository _usuarioMetodoPagoRepo;
        private readonly ITemporizadorOperacionRepository _temporizadorRepo;
        private readonly INotificacionService _notificaciones;

        public OperacionService(
            IOperacionRepository repo,
            IOfertaRepository ofertaRepo,
            IUsuarioRepository usuarioRepo,
            IUsuarioMetodoPagoRepository usuarioMetodoPagoRepo,
            ITemporizadorOperacionRepository temporizadorRepo,
            INotificacionService notificaciones)
        {
            _repo = repo;
            _ofertaRepo = ofertaRepo;
            _usuarioRepo = usuarioRepo;
            _usuarioMetodoPagoRepo = usuarioMetodoPagoRepo;
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
                OfertaMetodoPagoId = dto.OfertaMetodoPagoId,
                MetodoPagoId = dto.MetodoPagoId,
                UsuarioMetodoPagoId = dto.UsuarioMetodoPagoId,
                DatosPagoComprador = dto.DatosPagoComprador,
                ResumenPagoComprador = dto.ResumenPagoComprador,
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
            e.OfertaMetodoPagoId = dto.OfertaMetodoPagoId;
            e.MetodoPagoId = dto.MetodoPagoId;
            e.UsuarioMetodoPagoId = dto.UsuarioMetodoPagoId;
            e.DatosPagoComprador = dto.DatosPagoComprador;
            e.ResumenPagoComprador = dto.ResumenPagoComprador;
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

        public async Task<(OperacionDto? operacion, string? error)> IniciarTratoAsync(IniciarTratoDto dto, int compradorId)
        {
            var comprador = await _usuarioRepo.GetByIdAsync(compradorId);
            if (comprador == null)
                return (null, "El comprador no existe.");

            if (!PuedeOperar(comprador))
                return (null, "Debes verificar tu cuenta antes de iniciar una operación.");

            var oferta = await _ofertaRepo.GetByIdAsync(dto.OfertaId);
            if (oferta == null)
                return (null, "La oferta no existe.");

            if (oferta.Estado != "Activa")
                return (null, "Esta oferta ya no está disponible.");

            if (oferta.UsuarioId == compradorId)
                return (null, "No puedes iniciar un trato sobre tu propia oferta.");

            if (dto.Monto < oferta.MontoMinimo || dto.Monto > oferta.MontoMaximo)
                return (null, $"El monto debe estar entre {oferta.MontoMinimo} y {oferta.MontoMaximo}.");

            var ofertaMetodoPago = ResolverMetodoPagoOferta(oferta, dto);

            if (ofertaMetodoPago == null)
                return (null, "El método de pago seleccionado no pertenece a esta oferta.");

            var metodoPagoId = ofertaMetodoPago.MetodoPagoId;
            var metodoPagoNombre = ofertaMetodoPago.MetodoPago?.Nombre ?? "Método de pago";

            int? usuarioMetodoPagoId = null;
            string? datosPagoComprador = null;
            string? resumenPagoComprador = null;

            if (dto.UsuarioMetodoPagoId.HasValue)
            {
                var metodoGuardado = await _usuarioMetodoPagoRepo.GetByIdAsync(dto.UsuarioMetodoPagoId.Value);

                if (metodoGuardado == null || !metodoGuardado.Activo)
                    return (null, "El método guardado no existe o está inactivo.");

                if (metodoGuardado.UsuarioId != compradorId)
                    return (null, "No puedes usar un método guardado de otro usuario.");

                if (metodoGuardado.MetodoPagoId != metodoPagoId)
                    return (null, "El método guardado no coincide con el método elegido.");

                usuarioMetodoPagoId = metodoGuardado.Id;
                datosPagoComprador = metodoGuardado.DatosPago;
                resumenPagoComprador = metodoGuardado.ResumenPublico;
            }
            else
            {
                if (dto.DatosPagoComprador == null || dto.DatosPagoComprador.Count == 0)
                    return (null, "Ingresa tus datos de pago para esta operación.");

                datosPagoComprador = JsonSerializer.Serialize(dto.DatosPagoComprador);
                resumenPagoComprador = CrearResumenPagoComprador(metodoPagoNombre, dto.DatosPagoComprador);

                if (dto.GuardarMetodoComprador)
                {
                    var metodoGuardado = new UsuarioMetodoPago
                    {
                        UsuarioId = compradorId,
                        MetodoPagoId = metodoPagoId,
                        Alias = metodoPagoNombre,
                        DatosPago = datosPagoComprador,
                        ResumenPublico = resumenPagoComprador,
                        Activo = true,
                        FechaCreacion = DateTime.Now
                    };

                    var creado = await _usuarioMetodoPagoRepo.CreateAsync(metodoGuardado);
                    usuarioMetodoPagoId = creado.Id;
                }
            }

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
                OfertaMetodoPagoId = ofertaMetodoPago.Id,
                MetodoPagoId = metodoPagoId,
                UsuarioMetodoPagoId = usuarioMetodoPagoId,
                DatosPagoComprador = datosPagoComprador,
                ResumenPagoComprador = resumenPagoComprador,
                FechaInicio = ahora
            };

            var creada = await _repo.CreateAsync(nuevaOperacion);

            oferta.Estado = "Reservada";
            await _ofertaRepo.UpdateAsync(oferta);

            var temporizador = new TemporizadorOperacion
            {
                OperacionId = creada.Id,
                FechaInicio = ahora,
                FechaFin = ahora.AddMinutes(DuracionTemporizadorMinutos),
                Estado = "Activo"
            };

            await _temporizadorRepo.CreateAsync(temporizador);

            await NotificarAsync(compradorId, "Trato iniciado",
                $"Iniciaste un trato (código {nuevaOperacion.CodigoOperacion}). Tienes {DuracionTemporizadorMinutos} minutos para pagar.",
                creada.Id);

            await NotificarAsync(vendedorId, "Nuevo trato sobre tu oferta",
                $"Un comprador inició un trato sobre tu oferta (código {nuevaOperacion.CodigoOperacion}).",
                creada.Id);

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

            var ahora = DateTime.Now;

            operacion.Estado = "Cancelada";
            operacion.FechaFin ??= ahora;
            operacion.FechaLiberacion ??= ahora;

            await _repo.UpdateAsync(operacion);

            await ExpirarTemporizadorAsync(operacionId, "Cancelado");
            await LiberarOfertaAsync(operacion.OfertaId);

            var contraparte = usuarioId == operacion.CompradorId ? operacion.VendedorId : operacion.CompradorId;

            await NotificarAsync(contraparte, "Operación cancelada",
                $"La operación {operacion.CodigoOperacion} fue cancelada.",
                operacionId);

            return (true, null);
        }

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

            var oferta = await _ofertaRepo.GetByIdAsync(operacion.OfertaId);
            if (oferta != null && oferta.Estado == "Reservada")
            {
                oferta.Estado = "Cerrada";
                await _ofertaRepo.UpdateAsync(oferta);
            }

            await NotificarAsync(operacion.CompradorId, "Intercambio completado",
                $"El vendedor confirmó la recepción del pago. La operación {operacion.CodigoOperacion} se completó con éxito.",
                operacionId);

            return (true, null);
        }

        private static bool PuedeOperar(Usuario usuario)
        {
            if (usuario.Rol == "Administrador")
                return true;

            return usuario.EstadoVerificacion == "Verificado";
        }

        private static OfertaMetodoPago? ResolverMetodoPagoOferta(Oferta oferta, IniciarTratoDto dto)
        {
            if (dto.OfertaMetodoPagoId.HasValue)
            {
                return oferta.OfertaMetodoPagos
                    .FirstOrDefault(m => m.Id == dto.OfertaMetodoPagoId.Value);
            }

            if (dto.MetodoPagoId.HasValue)
            {
                return oferta.OfertaMetodoPagos
                    .FirstOrDefault(m => m.MetodoPagoId == dto.MetodoPagoId.Value);
            }

            return null;
        }

        private static string CrearResumenPagoComprador(string metodoPagoNombre, Dictionary<string, string> datos)
        {
            string? referencia = null;

            foreach (var key in new[] { "telefono", "cuenta", "correo", "ultimos4", "referencia" })
            {
                if (datos.TryGetValue(key, out var valor) && !string.IsNullOrWhiteSpace(valor))
                {
                    referencia = valor.Trim();
                    break;
                }
            }

            referencia ??= datos.Values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? "registrado";

            if (referencia.Length <= 4)
                return $"{metodoPagoNombre}: {referencia}";

            return $"{metodoPagoNombre}: ****{referencia[^4..]}";
        }

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
                    e.FechaFin ??= ahora;
                    e.FechaLiberacion ??= ahora;

                    await _repo.UpdateAsync(e);
                    await LiberarOfertaAsync(e.OfertaId);

                    await NotificarAsync(e.CompradorId, "Operación expirada",
                        $"El tiempo para pagar la operación {e.CodigoOperacion} expiró.",
                        e.Id);

                    await NotificarAsync(e.VendedorId, "Operación expirada",
                        $"La operación {e.CodigoOperacion} expiró sin pago. Tu oferta vuelve a estar activa.",
                        e.Id);
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

        private static OperacionDto MapToDto(Operacion e)
        {
            return new OperacionDto
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
                OfertaMetodoPagoId = e.OfertaMetodoPagoId,
                MetodoPagoId = e.MetodoPagoId,
                MetodoPagoNombre = e.MetodoPago?.Nombre
                    ?? e.OfertaMetodoPago?.MetodoPago?.Nombre
                    ?? string.Empty,
                UsuarioMetodoPagoId = e.UsuarioMetodoPagoId,
                DatosPagoComprador = e.DatosPagoComprador,
                ResumenPagoComprador = e.ResumenPagoComprador,
                FechaInicio = e.FechaInicio,
                FechaFin = e.FechaFin,
                FechaLiberacion = e.FechaLiberacion
            };
        }
    }
}