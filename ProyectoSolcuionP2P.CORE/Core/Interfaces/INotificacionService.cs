using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface INotificacionService
    {
        Task<IEnumerable<NotificacionDto>> GetAllAsync();
        Task<NotificacionDto?> GetByIdAsync(int id);
        Task<NotificacionDto> CreateAsync(NotificacionDto dto);
        Task<bool> UpdateAsync(int id, NotificacionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
