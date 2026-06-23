using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOfertaService
    {
        Task<IEnumerable<OfertaDto>> GetAllAsync();
        Task<OfertaDto?> GetByIdAsync(int id);
        Task<OfertaDto> CreateAsync(OfertaDto dto);
        Task<bool> UpdateAsync(int id, OfertaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
