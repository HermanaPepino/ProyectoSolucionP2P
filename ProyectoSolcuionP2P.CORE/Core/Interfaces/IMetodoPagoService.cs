using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMetodoPagoService
    {
        Task<IEnumerable<MetodoPagoDto>> GetAllAsync();
        Task<MetodoPagoDto?> GetByIdAsync(int id);
        Task<MetodoPagoDto> CreateAsync(MetodoPagoDto dto);
        Task<bool> UpdateAsync(int id, MetodoPagoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
