using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IComprobantePagoService
    {
        Task<IEnumerable<ComprobantePago>> GetAllAsync();
        Task<ComprobantePago?> GetByIdAsync(int id);
        Task<ComprobantePago> CreateAsync(ComprobantePago entity);
        Task UpdateAsync(ComprobantePago entity);
        Task DeleteAsync(int id);
    }
}
