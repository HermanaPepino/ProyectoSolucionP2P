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

        // HU-014: abre la disputa, congela la operación y notifica
        Task<(DisputaDto? disputa, string? error)> AbrirAsync(int operacionId, string motivo, int usuarioId);
    }
}