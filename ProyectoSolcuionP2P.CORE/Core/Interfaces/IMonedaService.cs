using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMonedaService
    {
        Task<IEnumerable<MonedaDto>> GetAllAsync();
        Task<MonedaDto?> GetByIdAsync(int id);
        Task<MonedaDto> CreateAsync(MonedaDto dto);
        Task<bool> UpdateAsync(int id, MonedaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}