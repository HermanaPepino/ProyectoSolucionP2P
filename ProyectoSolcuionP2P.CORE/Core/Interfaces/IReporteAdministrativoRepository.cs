using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IReporteAdministrativoRepository
    {
        Task<IEnumerable<ReporteAdministrativo>> GetAllAsync();
        Task<ReporteAdministrativo?> GetByIdAsync(int id);
        Task<ReporteAdministrativo> CreateAsync(ReporteAdministrativo entity);
        Task UpdateAsync(ReporteAdministrativo entity);
        Task DeleteAsync(int id);
    }
}
