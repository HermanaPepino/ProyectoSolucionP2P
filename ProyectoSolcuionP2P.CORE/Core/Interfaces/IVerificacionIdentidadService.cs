using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IVerificacionIdentidadService
    {
        Task<IEnumerable<VerificacionIdentidadDto>> GetAllAsync();
        Task<VerificacionIdentidadDto?> GetByIdAsync(int id);
        Task<VerificacionIdentidadDto> CreateAsync(VerificacionIdentidadDto dto);
        Task<bool> UpdateAsync(int id, VerificacionIdentidadDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
