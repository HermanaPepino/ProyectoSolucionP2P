using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IVerificacionIdentidadRepository
    {
        Task<IEnumerable<VerificacionIdentidad>> GetAllAsync();

        Task<IEnumerable<VerificacionIdentidad>> GetPendientesAsync();

        Task<VerificacionIdentidad?> GetByIdAsync(int id);

        Task<VerificacionIdentidad?> GetByUsuarioIdAsync(int usuarioId);

        Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity);

        Task UpdateAsync(VerificacionIdentidad entity);

        Task DeleteAsync(int id);
    }
}
