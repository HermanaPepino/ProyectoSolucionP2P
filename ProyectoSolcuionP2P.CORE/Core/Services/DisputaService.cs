using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class DisputaService : IDisputaService
    {
        private readonly IDisputaRepository _repo;
        public DisputaService(IDisputaRepository repo) { _repo = repo; }

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
