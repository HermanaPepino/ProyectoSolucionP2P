using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class ComprobantePagoRepository : IComprobantePagoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public ComprobantePagoRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<ComprobantePago>> GetAllAsync()
            => await _db.Set<ComprobantePago>().AsNoTracking().ToListAsync();

        public async Task<ComprobantePago?> GetByIdAsync(int id)
            => await _db.Set<ComprobantePago>().FindAsync(id);

        public async Task<ComprobantePago> CreateAsync(ComprobantePago entity)
        {
            _db.Set<ComprobantePago>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ComprobantePago entity)
        {
            _db.Set<ComprobantePago>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<ComprobantePago>().FindAsync(id);
            if (ent == null) return;
            _db.Set<ComprobantePago>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
