using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class DisputaRepository : IDisputaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public DisputaRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Disputa>> GetAllAsync()
            => await _db.Set<Disputa>().AsNoTracking().ToListAsync();

        public async Task<Disputa?> GetByIdAsync(int id)
            => await _db.Set<Disputa>().FindAsync(id);

        public async Task<Disputa> CreateAsync(Disputa entity)
        {
            _db.Set<Disputa>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Disputa entity)
        {
            _db.Set<Disputa>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Disputa>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Disputa>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
