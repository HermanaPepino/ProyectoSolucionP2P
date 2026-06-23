using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMonedaRepository
    {
        Task<IEnumerable<Moneda>> GetAllAsync();
        Task<Moneda?> GetByIdAsync(int id);
        Task<Moneda> CreateAsync(Moneda entity);
        Task UpdateAsync(Moneda entity);
        Task DeleteAsync(int id);
    }
}