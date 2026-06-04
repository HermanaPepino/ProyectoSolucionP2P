using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class MensajeService : IMensajeService
    {
        private readonly IMensajeRepository _repo;

        public MensajeService(IMensajeRepository repo)
        {
            _repo = repo;
        }

        public Task<Mensaje> CreateAsync(Mensaje entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Mensaje>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Mensaje?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Mensaje entity) => _repo.UpdateAsync(entity);
    }
}
