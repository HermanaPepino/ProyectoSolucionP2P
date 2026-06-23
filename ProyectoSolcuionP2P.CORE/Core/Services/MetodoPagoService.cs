using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly IMetodoPagoRepository _repo;
        public MetodoPagoService(IMetodoPagoRepository repo) { _repo = repo; }

        public async Task<IEnumerable<MetodoPagoDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<MetodoPagoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<MetodoPagoDto> CreateAsync(MetodoPagoDto dto)
        {
            var e = new MetodoPago { Nombre = dto.Nombre, Activo = dto.Activo };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, MetodoPagoDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.Nombre = dto.Nombre;
            e.Activo = dto.Activo;
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

        private static MetodoPagoDto MapToDto(MetodoPago e) => new MetodoPagoDto
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Activo = e.Activo
        };
    }
}
