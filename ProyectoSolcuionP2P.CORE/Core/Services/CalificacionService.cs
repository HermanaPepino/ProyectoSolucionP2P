using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ICalificacionRepository _repo;
        public CalificacionService(ICalificacionRepository repo) { _repo = repo; }

        public async Task<IEnumerable<CalificacionDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<CalificacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<CalificacionDto> CreateAsync(CalificacionDto dto)
        {
            var e = new Calificacion 
            { 
                OperacionId = dto.OperacionId,
                UsuarioCalificadoId = dto.UsuarioCalificadoId,
                Puntaje = dto.Puntaje,
                Comentario = dto.Comentario,
                FechaRegistro = dto.FechaRegistro
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, CalificacionDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OperacionId = dto.OperacionId;
            e.UsuarioCalificadoId = dto.UsuarioCalificadoId;
            e.Puntaje = dto.Puntaje;
            e.Comentario = dto.Comentario;
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

        private static CalificacionDto MapToDto(Calificacion e) => new CalificacionDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            UsuarioCalificadoId = e.UsuarioCalificadoId,
            Puntaje = e.Puntaje,
            Comentario = e.Comentario,
            FechaRegistro = e.FechaRegistro
        };
    }
}
