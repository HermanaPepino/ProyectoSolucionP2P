using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IDisputaRepository
    {
        Task<IEnumerable<Disputa>> GetAllAsync();
        Task<Disputa?> GetByIdAsync(int id);
        Task<Disputa> CreateAsync(Disputa entity);
        Task UpdateAsync(Disputa entity);
        Task DeleteAsync(int id);
    }
}
