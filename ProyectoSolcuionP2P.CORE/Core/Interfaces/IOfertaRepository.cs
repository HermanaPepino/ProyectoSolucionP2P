using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOfertaRepository
    {
        Task<IEnumerable<Oferta>> GetAllAsync();
        Task<Oferta?> GetByIdAsync(int id);
        Task<Oferta> CreateAsync(Oferta entity);
        Task UpdateAsync(Oferta entity);
        Task DeleteAsync(int id);
    }
}
