using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IUsuarioMetodoPagoRepository
    {
        Task<IEnumerable<UsuarioMetodoPago>> GetByUsuarioIdAsync(int usuarioId);

        Task<UsuarioMetodoPago?> GetByIdAsync(int id);

        Task<UsuarioMetodoPago> CreateAsync(UsuarioMetodoPago entity);

        Task UpdateAsync(UsuarioMetodoPago entity);

        Task DeleteAsync(int id);
    }
}