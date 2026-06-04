using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMensajeRepository
    {
        Task<IEnumerable<Mensaje>> GetAllAsync();
        Task<Mensaje?> GetByIdAsync(int id);
        Task<Mensaje> CreateAsync(Mensaje entity);
        Task UpdateAsync(Mensaje entity);
        Task DeleteAsync(int id);
    }
}
