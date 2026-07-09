using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IDisputaRepository
    {
        Task<IEnumerable<Disputa>> GetAllAsync();
        Task<Disputa?> GetByIdAsync(int id);

        // Una operación solo debe tener una disputa en todo su ciclo de vida.
        Task<Disputa?> GetByOperacionIdAsync(int operacionId);

        // Historial del usuario autenticado, incluyendo abiertas y cerradas recientes.
        Task<IEnumerable<Disputa>> GetByUsuarioIdAsync(int usuarioId);

        Task<Disputa> CreateAsync(Disputa entity);
        Task UpdateAsync(Disputa entity);
        Task DeleteAsync(int id);
    }
}