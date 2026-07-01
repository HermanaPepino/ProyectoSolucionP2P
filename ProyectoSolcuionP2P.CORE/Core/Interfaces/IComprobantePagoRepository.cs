using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IComprobantePagoRepository
    {
        Task<IEnumerable<ComprobantePago>> GetAllAsync();
        Task<ComprobantePago?> GetByIdAsync(int id);
        Task<ComprobantePago?> GetByOperacionIdAsync(int operacionId);
        Task<ComprobantePago> CreateAsync(ComprobantePago entity);
        Task UpdateAsync(ComprobantePago entity);
        Task DeleteAsync(int id);
    }
}