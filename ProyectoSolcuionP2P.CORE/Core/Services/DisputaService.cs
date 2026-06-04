using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class DisputaService : IDisputaService
    {
        private readonly IDisputaRepository _repo;

        public DisputaService(IDisputaRepository repo)
        {
            _repo = repo;
        }

        public Task<Disputa> CreateAsync(Disputa entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Disputa>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Disputa?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Disputa entity) => _repo.UpdateAsync(entity);
    }
}
