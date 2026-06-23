using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IComprobantePagoService
    {
        Task<IEnumerable<ComprobantePagoDto>> GetAllAsync();
        Task<ComprobantePagoDto?> GetByIdAsync(int id);
        Task<ComprobantePagoDto> CreateAsync(ComprobantePagoDto dto);
        Task<bool> UpdateAsync(int id, ComprobantePagoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
