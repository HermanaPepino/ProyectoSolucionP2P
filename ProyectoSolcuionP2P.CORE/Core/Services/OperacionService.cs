using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OperacionService : IOperacionService
    {
        private readonly IOperacionRepository _repo;

        public OperacionService(IOperacionRepository repo)
        {
            _repo = repo;
        }

        public Task<Operacion> CreateAsync(Operacion entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Operacion>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Operacion?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Operacion entity) => _repo.UpdateAsync(entity);
    }
}
