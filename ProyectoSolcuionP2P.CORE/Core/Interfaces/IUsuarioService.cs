using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task<UsuarioDto?> RegistrarAsync(UsuarioRegistroDto dto);   // HU-001
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);            // HU-002
        Task<bool> UpdateAsync(int id, UsuarioRegistroDto dto);
        Task<bool> DeleteAsync(int id);
        Task<UsuarioReputacionDto> GetReputacionAsync(int usuarioId);
    }
}