using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public Task<Usuario> CreateAsync(Usuario entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Usuario>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Usuario?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Usuario entity) => _repo.UpdateAsync(entity);
    }
}
