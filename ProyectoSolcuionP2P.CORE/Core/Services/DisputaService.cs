using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class DisputaService : IDisputaService
    {
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

        public async Task<DisputaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<DisputaDto> CreateAsync(DisputaDto dto)
        {
            var e = new Disputa
            {
                OperacionId = dto.OperacionId,
                Motivo = dto.Motivo,
                Estado = dto.Estado,
                Resolucion = dto.Resolucion,
                FechaRegistro = dto.FechaRegistro
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, DisputaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OperacionId = dto.OperacionId;
            e.Motivo = dto.Motivo;
            e.Estado = dto.Estado;
            e.Resolucion = dto.Resolucion;
            e.FechaRegistro = dto.FechaRegistro;
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

        // HU-014: Abrir disputa
        public async Task<(DisputaDto? disputa, string? error)> AbrirAsync(int operacionId, string motivo, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return (null, "Debes indicar un motivo.");

            var operacion = await _operacionRepo.GetByIdAsync(operacionId);
            if (operacion == null) return (null, "La operación no existe.");

            if (operacion.CompradorId != usuarioId && operacion.VendedorId != usuarioId)
                return (null, "No tienes permiso sobre esta operación.");

            if (operacion.Estado is not ("En proceso" or "Pago enviado"))
                return (null, "Solo se pueden disputar operaciones activas o con pago enviado.");

            var disputa = new Disputa
            {
                OperacionId = operacionId,
                Motivo = motivo,
                Estado = "Abierta",
                FechaRegistro = DateTime.Now
            };
            var creada = await _repo.CreateAsync(disputa);

            // La operación queda congelada hasta que el administrador resuelva
            operacion.Estado = "En disputa";
            await _operacionRepo.UpdateAsync(operacion);

            var contraparte = usuarioId == operacion.CompradorId ? operacion.VendedorId : operacion.CompradorId;
            await _notificaciones.CreateAsync(new NotificacionDto
            {
                UsuarioId = contraparte,
                Titulo = "Se abrió una disputa",
                Mensaje = $"Se abrió una disputa sobre la operación {operacion.CodigoOperacion}. Un administrador la revisará.",
                Leida = false,
                OperacionId = operacionId,
                FechaCreacion = DateTime.Now
            });

            return (MapToDto(creada), null);
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
    }
}