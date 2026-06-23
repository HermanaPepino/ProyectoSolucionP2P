using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ICalificacionRepository
    {
        Task<IEnumerable<Calificacion>> GetAllAsync();
        Task<Calificacion?> GetByIdAsync(int id);
        Task<Calificacion> CreateAsync(Calificacion entity);
        Task UpdateAsync(Calificacion entity);
        Task DeleteAsync(int id);
    }
}
