using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface ITemporizadorOperacionService
    {
        Task<IEnumerable<TemporizadorOperacionDto>> GetAllAsync();
        Task<TemporizadorOperacionDto?> GetByIdAsync(int id);
        Task<TemporizadorOperacionDto> CreateAsync(TemporizadorOperacionDto dto);
        Task<bool> UpdateAsync(int id, TemporizadorOperacionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
