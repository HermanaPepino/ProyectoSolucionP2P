using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(int id);
        Task<Notificacion> CreateAsync(Notificacion entity);
        Task UpdateAsync(Notificacion entity);
        Task DeleteAsync(int id);
    }
}
