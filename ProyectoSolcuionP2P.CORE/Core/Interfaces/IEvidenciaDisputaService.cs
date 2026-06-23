using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IEvidenciaDisputaService
    {
        Task<IEnumerable<EvidenciaDisputaDto>> GetAllAsync();
        Task<EvidenciaDisputaDto?> GetByIdAsync(int id);
        Task<EvidenciaDisputaDto> CreateAsync(EvidenciaDisputaDto dto);
        Task<bool> UpdateAsync(int id, EvidenciaDisputaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
