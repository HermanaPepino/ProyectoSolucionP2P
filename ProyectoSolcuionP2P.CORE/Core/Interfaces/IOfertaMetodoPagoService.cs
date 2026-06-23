using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOfertaMetodoPagoService
    {
        Task<IEnumerable<OfertaMetodoPagoDto>> GetAllAsync();
        Task<OfertaMetodoPagoDto?> GetByIdAsync(int id);
        Task<OfertaMetodoPagoDto> CreateAsync(OfertaMetodoPagoDto dto);
        Task<bool> UpdateAsync(int id, OfertaMetodoPagoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
