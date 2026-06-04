using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ICalificacionRepository _repo;

        public CalificacionService(ICalificacionRepository repo)
        {
            _repo = repo;
        }

        public Task<Calificacion> CreateAsync(Calificacion entity) => _repo.CreateAsync(entity);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Calificacion>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Calificacion?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task UpdateAsync(Calificacion entity) => _repo.UpdateAsync(entity);
    }
}
