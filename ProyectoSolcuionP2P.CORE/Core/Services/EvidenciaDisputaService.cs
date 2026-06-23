using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class EvidenciaDisputaService : IEvidenciaDisputaService
    {
        private readonly IEvidenciaDisputaRepository _repo;
        public EvidenciaDisputaService(IEvidenciaDisputaRepository repo) { _repo = repo; }

        public async Task<IEnumerable<EvidenciaDisputaDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<EvidenciaDisputaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<EvidenciaDisputaDto> CreateAsync(EvidenciaDisputaDto dto)
        {
            var e = new EvidenciaDisputa 
            { 
                DisputaId = dto.DisputaId,
                RutaArchivo = dto.RutaArchivo,
                FechaSubida = dto.FechaSubida
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, EvidenciaDisputaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.DisputaId = dto.DisputaId;
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

        private static EvidenciaDisputaDto MapToDto(EvidenciaDisputa e) => new EvidenciaDisputaDto
        {
            Id = e.Id,
            DisputaId = e.DisputaId,
            RutaArchivo = e.RutaArchivo,
            FechaSubida = e.FechaSubida
        };
    }
}
