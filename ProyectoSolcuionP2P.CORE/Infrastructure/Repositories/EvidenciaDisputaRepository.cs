using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class EvidenciaDisputaRepository : IEvidenciaDisputaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public EvidenciaDisputaRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<EvidenciaDisputa>> GetAllAsync()
            => await _db.Set<EvidenciaDisputa>().AsNoTracking().ToListAsync();

        public async Task<EvidenciaDisputa?> GetByIdAsync(int id)
            => await _db.Set<EvidenciaDisputa>().FindAsync(id);

        public async Task<EvidenciaDisputa> CreateAsync(EvidenciaDisputa entity)
        {
            _db.Set<EvidenciaDisputa>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(EvidenciaDisputa entity)
        {
            _db.Set<EvidenciaDisputa>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<EvidenciaDisputa>().FindAsync(id);
            if (ent == null) return;
            _db.Set<EvidenciaDisputa>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
