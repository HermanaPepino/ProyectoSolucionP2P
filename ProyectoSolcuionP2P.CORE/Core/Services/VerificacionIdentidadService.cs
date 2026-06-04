using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class VerificacionIdentidadService : IVerificacionIdentidadService
    {
        private readonly IVerificacionIdentidadRepository _repo;

        public VerificacionIdentidadService(IVerificacionIdentidadRepository repo)
        {
            _repo = repo;
        }

        public Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<VerificacionIdentidad>> GetAllAsync() => _repo.GetAllAsync();

        public Task<VerificacionIdentidad?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(VerificacionIdentidad entity) => _repo.UpdateAsync(entity);
    }
}
