using ProyectoSolucionP2P.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ICalificacionService
    {
        Task<IEnumerable<Calificacion>> GetAllAsync();
        Task<Calificacion?> GetByIdAsync(int id);
        Task<Calificacion> CreateAsync(Calificacion entity);
        Task UpdateAsync(Calificacion entity);
        Task DeleteAsync(int id);
    }
}
