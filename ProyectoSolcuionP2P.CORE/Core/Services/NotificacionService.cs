using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _repo;
        public NotificacionService(INotificacionRepository repo) { _repo = repo; }

        public async Task<IEnumerable<NotificacionDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<NotificacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<NotificacionDto> CreateAsync(NotificacionDto dto)
        {
            var e = new Notificacion 
            { 
                UsuarioId = dto.UsuarioId,
                Titulo = dto.Titulo,
                Mensaje = dto.Mensaje,
                Leida = dto.Leida,
                OperacionId = dto.OperacionId,
                FechaCreacion = dto.FechaCreacion
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, NotificacionDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.UsuarioId = dto.UsuarioId;
            e.Titulo = dto.Titulo;
            e.Mensaje = dto.Mensaje;
            e.Leida = dto.Leida;
            e.OperacionId = dto.OperacionId;
            e.FechaCreacion = dto.FechaCreacion;
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

        private static NotificacionDto MapToDto(Notificacion e) => new NotificacionDto
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            Titulo = e.Titulo,
            Mensaje = e.Mensaje,
            Leida = e.Leida,
            OperacionId = e.OperacionId,
            FechaCreacion = e.FechaCreacion
        };
    }
}
