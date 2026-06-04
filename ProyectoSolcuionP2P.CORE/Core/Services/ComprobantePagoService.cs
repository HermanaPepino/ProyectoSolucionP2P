using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class ComprobantePagoService : IComprobantePagoService
    {
        private readonly IComprobantePagoRepository _repo;

        public ComprobantePagoService(IComprobantePagoRepository repo)
        {
            _repo = repo;
        }

        public Task<ComprobantePago> CreateAsync(ComprobantePago entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<ComprobantePago>> GetAllAsync() => _repo.GetAllAsync();

        public Task<ComprobantePago?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(ComprobantePago entity) => _repo.UpdateAsync(entity);
    }
}
