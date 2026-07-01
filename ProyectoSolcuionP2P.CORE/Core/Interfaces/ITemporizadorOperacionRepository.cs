using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ITemporizadorOperacionRepository
    {
        Task<IEnumerable<TemporizadorOperacion>> GetAllAsync();
        Task<TemporizadorOperacion?> GetByIdAsync(int id);
        Task<TemporizadorOperacion?> GetByOperacionIdAsync(int operacionId);
        Task<TemporizadorOperacion> CreateAsync(TemporizadorOperacion entity);
        Task UpdateAsync(TemporizadorOperacion entity);
        Task DeleteAsync(int id);
    }
}