using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class CalificacionRepository : ICalificacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public CalificacionRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Calificacion>> GetAllAsync()
            => await _db.Set<Calificacion>().AsNoTracking().ToListAsync();

        public async Task<Calificacion?> GetByIdAsync(int id)
            => await _db.Set<Calificacion>().FindAsync(id);

        public async Task<Calificacion> CreateAsync(Calificacion entity)
        {
            _db.Set<Calificacion>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Calificacion entity)
        {
            _db.Set<Calificacion>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Calificacion>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Calificacion>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
