using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ITemporizadorOperacionService
    {
        Task<IEnumerable<TemporizadorOperacion>> GetAllAsync();
        Task<TemporizadorOperacion?> GetByIdAsync(int id);
        Task<TemporizadorOperacion> CreateAsync(TemporizadorOperacion entity);
        Task UpdateAsync(TemporizadorOperacion entity);
        Task DeleteAsync(int id);
    }
}
