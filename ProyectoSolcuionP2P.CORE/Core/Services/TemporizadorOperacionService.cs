using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class TemporizadorOperacionService : ITemporizadorOperacionService
    {
        private readonly ITemporizadorOperacionRepository _repo;
        public TemporizadorOperacionService(ITemporizadorOperacionRepository repo) { _repo = repo; }

        public async Task<IEnumerable<TemporizadorOperacionDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<TemporizadorOperacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<TemporizadorOperacionDto> CreateAsync(TemporizadorOperacionDto dto)
        {
            var e = new TemporizadorOperacion 
            { 
                OperacionId = dto.OperacionId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Estado = dto.Estado
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, TemporizadorOperacionDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OperacionId = dto.OperacionId;
            e.FechaInicio = dto.FechaInicio;
            e.FechaFin = dto.FechaFin;
            e.Estado = dto.Estado;
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

        private static TemporizadorOperacionDto MapToDto(TemporizadorOperacion e) => new TemporizadorOperacionDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            FechaInicio = e.FechaInicio,
            FechaFin = e.FechaFin,
            Estado = e.Estado
        };
    }
}
