using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class TemporizadorOperacionService : ITemporizadorOperacionService
    {
        private readonly ITemporizadorOperacionRepository _repo;

        public TemporizadorOperacionService(ITemporizadorOperacionRepository repo)
        {
            _repo = repo;
        }

        public Task<TemporizadorOperacion> CreateAsync(TemporizadorOperacion entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<TemporizadorOperacion>> GetAllAsync() => _repo.GetAllAsync();

        public Task<TemporizadorOperacion?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(TemporizadorOperacion entity) => _repo.UpdateAsync(entity);
    }
}
