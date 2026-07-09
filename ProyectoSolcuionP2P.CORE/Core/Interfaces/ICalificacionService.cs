using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ICalificacionService
    {
        Task<IEnumerable<CalificacionDto>> GetAllAsync();
        Task<CalificacionDto?> GetByIdAsync(int id);
        Task<CalificacionDto> CreateAsync(CalificacionDto dto);
        Task<CalificacionDto> CreateAsync(CalificacionDto dto, int usuarioActualId);
        Task<IEnumerable<CalificacionHistorialDto>> GetMisCalificacionesAsync(int usuarioActualId, int dias = 30);
        Task<bool> UpdateAsync(int id, CalificacionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}