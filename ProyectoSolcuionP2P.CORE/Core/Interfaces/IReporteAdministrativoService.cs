using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IReporteAdministrativoService
    {
        Task<IEnumerable<ReporteAdministrativoDto>> GetAllAsync();
        Task<ReporteAdministrativoDto?> GetByIdAsync(int id);
        Task<ReporteAdministrativoDto> CreateAsync(ReporteAdministrativoDto dto);
        Task<bool> UpdateAsync(int id, ReporteAdministrativoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
