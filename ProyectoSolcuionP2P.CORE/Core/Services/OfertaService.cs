using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository _repo;

        public OfertaService(IOfertaRepository repo)
        {
            _repo = repo;
        }

        public Task<Oferta> CreateAsync(Oferta entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Oferta>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Oferta?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Oferta entity) => _repo.UpdateAsync(entity);
    }
}
