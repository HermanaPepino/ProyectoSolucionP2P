using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class MensajeRepository : IMensajeRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public MensajeRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Mensaje>> GetAllAsync()
            => await _db.Set<Mensaje>()
                .Include(m => m.Remitente)
                .Include(m => m.Destinatario)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Mensaje?> GetByIdAsync(int id)
            => await _db.Set<Mensaje>()
                .Include(m => m.Remitente)
                .Include(m => m.Destinatario)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Mensaje> CreateAsync(Mensaje entity)
        {
            _db.Set<Mensaje>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Mensaje entity)
        {
            _db.Set<Mensaje>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Mensaje>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Mensaje>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
