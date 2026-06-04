using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<Operacion>> GetAllAsync();
        Task<Operacion?> GetByIdAsync(int id);
        Task<Operacion> CreateAsync(Operacion entity);
        Task UpdateAsync(Operacion entity);
        Task DeleteAsync(int id);
    }
}
