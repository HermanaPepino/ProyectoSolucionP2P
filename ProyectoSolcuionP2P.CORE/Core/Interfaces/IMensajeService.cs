using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IMensajeService
    {
        Task<IEnumerable<MensajeDto>> GetAllAsync();
        Task<MensajeDto?> GetByIdAsync(int id);
        Task<MensajeDto> CreateAsync(MensajeDto dto);
        Task<bool> UpdateAsync(int id, MensajeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
