using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerarToken(Usuario usuario);
    }
}