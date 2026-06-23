using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOperacionRepository
    {
        Task<IEnumerable<Operacion>> GetAllAsync();
        Task<Operacion?> GetByIdAsync(int id);
        Task<Operacion> CreateAsync(Operacion entity);
        Task UpdateAsync(Operacion entity);
        Task DeleteAsync(int id);
    }
}
