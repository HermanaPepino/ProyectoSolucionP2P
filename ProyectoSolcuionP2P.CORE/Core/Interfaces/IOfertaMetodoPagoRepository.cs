using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOfertaMetodoPagoRepository
    {
        Task<IEnumerable<OfertaMetodoPago>> GetAllAsync();
        Task<OfertaMetodoPago?> GetByIdAsync(int id);
        Task<OfertaMetodoPago> CreateAsync(OfertaMetodoPago entity);
        Task UpdateAsync(OfertaMetodoPago entity);
        Task DeleteAsync(int id);
    }
}
