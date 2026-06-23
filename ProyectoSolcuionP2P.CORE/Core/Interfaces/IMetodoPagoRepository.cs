using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMetodoPagoRepository
    {
        Task<IEnumerable<MetodoPago>> GetAllAsync();
        Task<MetodoPago?> GetByIdAsync(int id);
        Task<MetodoPago> CreateAsync(MetodoPago entity);
        Task UpdateAsync(MetodoPago entity);
        Task DeleteAsync(int id);
    }
}
