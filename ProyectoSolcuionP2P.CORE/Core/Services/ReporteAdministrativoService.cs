using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class ReporteAdministrativoService : IReporteAdministrativoService
    {
        private readonly IReporteAdministrativoRepository _repo;

        public ReporteAdministrativoService(IReporteAdministrativoRepository repo)
        {
            _repo = repo;
        }

        public Task<ReporteAdministrativo> CreateAsync(ReporteAdministrativo entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<ReporteAdministrativo>> GetAllAsync() => _repo.GetAllAsync();

        public Task<ReporteAdministrativo?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(ReporteAdministrativo entity) => _repo.UpdateAsync(entity);
    }
}
