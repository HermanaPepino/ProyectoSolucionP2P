using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IUsuarioMetodoPagoService
    {
        Task<IEnumerable<UsuarioMetodoPagoDto>> GetByUsuarioIdAsync(int usuarioId);

        Task<UsuarioMetodoPagoDto?> GetByIdAsync(int id);

        Task<UsuarioMetodoPagoDto> CreateAsync(UsuarioMetodoPagoCreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}