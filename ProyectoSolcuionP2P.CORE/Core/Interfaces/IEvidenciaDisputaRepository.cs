using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IEvidenciaDisputaRepository
    {
        Task<IEnumerable<EvidenciaDisputa>> GetAllAsync();
        Task<EvidenciaDisputa?> GetByIdAsync(int id);
        Task<EvidenciaDisputa> CreateAsync(EvidenciaDisputa entity);
        Task UpdateAsync(EvidenciaDisputa entity);
        Task DeleteAsync(int id);
    }
}
