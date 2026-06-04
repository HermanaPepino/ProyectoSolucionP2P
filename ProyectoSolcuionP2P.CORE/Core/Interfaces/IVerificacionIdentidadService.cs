using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IVerificacionIdentidadService
    {
        Task<IEnumerable<VerificacionIdentidad>> GetAllAsync();
        Task<VerificacionIdentidad?> GetByIdAsync(int id);
        Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity);
        Task UpdateAsync(VerificacionIdentidad entity);
        Task DeleteAsync(int id);
    }
}
