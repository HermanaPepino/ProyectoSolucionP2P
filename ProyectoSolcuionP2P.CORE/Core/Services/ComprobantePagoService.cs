using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class ComprobantePagoService : IComprobantePagoService
    {
        private readonly IComprobantePagoRepository _repo;
        public ComprobantePagoService(IComprobantePagoRepository repo) { _repo = repo; }

        public async Task<IEnumerable<ComprobantePagoDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<ComprobantePagoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<ComprobantePagoDto> CreateAsync(ComprobantePagoDto dto)
        {
            var e = new ComprobantePago 
            { 
                OperacionId = dto.OperacionId,
                RutaArchivo = dto.RutaArchivo,
                FechaSubida = dto.FechaSubida
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, ComprobantePagoDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OperacionId = dto.OperacionId;
            e.RutaArchivo = dto.RutaArchivo;
            e.FechaSubida = dto.FechaSubida;
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

        private static ComprobantePagoDto MapToDto(ComprobantePago e) => new ComprobantePagoDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            RutaArchivo = e.RutaArchivo,
            FechaSubida = e.FechaSubida
        };
    }
}
