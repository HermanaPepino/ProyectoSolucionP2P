using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByCorreoAsync(string correo);   // para login y validar correo único
        Task<Usuario> CreateAsync(Usuario entity);
        Task UpdateAsync(Usuario entity);
        Task DeleteAsync(int id);
    }
}