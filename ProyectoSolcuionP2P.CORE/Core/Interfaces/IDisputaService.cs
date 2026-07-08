using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IDisputaService
    {
        Task<IEnumerable<DisputaDto>> GetAllAsync();
        Task<DisputaDto?> GetByIdAsync(int id);
        Task<DisputaDto> CreateAsync(DisputaDto dto);
        Task<bool> UpdateAsync(int id, DisputaDto dto);
        Task<bool> DeleteAsync(int id);

        // Abre la disputa, valida permisos, evita duplicados y respeta el plazo de 30 días.
        Task<(DisputaDto? disputa, string? error)> AbrirAsync(int operacionId, string motivo, int usuarioId);

        Task<IEnumerable<DisputaHistorialDto>> GetMisDisputasAsync(int usuarioId);
        Task<DisputaHistorialDto?> GetByOperacionForUserAsync(int operacionId, int usuarioId);
    }
}