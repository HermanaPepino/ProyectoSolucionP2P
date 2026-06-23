using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly IMonedaRepository _repo;
        public MonedaService(IMonedaRepository repo) { _repo = repo; }

        public async Task<IEnumerable<MonedaDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<MonedaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<MonedaDto> CreateAsync(MonedaDto dto)
        {
            var e = new Moneda { Codigo = dto.Codigo, Nombre = dto.Nombre };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, MonedaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.Codigo = dto.Codigo;
            e.Nombre = dto.Nombre;
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

        private static MonedaDto MapToDto(Moneda e) => new MonedaDto
        {
            Id = e.Id,
            Codigo = e.Codigo,
            Nombre = e.Nombre
        };
    }
}