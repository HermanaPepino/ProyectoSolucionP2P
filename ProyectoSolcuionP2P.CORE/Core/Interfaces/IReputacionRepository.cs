using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IReputacionRepository
    {
        Task<UsuarioReputacionDto> ObtenerReputacionAsync(int usuarioId);
    }
}