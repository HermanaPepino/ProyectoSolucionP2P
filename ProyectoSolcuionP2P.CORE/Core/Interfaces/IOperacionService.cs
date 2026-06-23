using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<OperacionDto>> GetAllAsync();
        Task<OperacionDto?> GetByIdAsync(int id);
        Task<OperacionDto> CreateAsync(OperacionDto dto);
        Task<bool> UpdateAsync(int id, OperacionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
