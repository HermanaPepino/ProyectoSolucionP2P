using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public OfertaRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Oferta>> GetAllAsync()
            => await _db.Set<Oferta>()
                .Include(o => o.Usuario)
                .Include(o => o.MonedaOrigen)
                .Include(o => o.MonedaDestino)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Oferta?> GetByIdAsync(int id)
            => await _db.Set<Oferta>()
                .Include(o => o.Usuario)
                .Include(o => o.MonedaOrigen)
                .Include(o => o.MonedaDestino)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Oferta> CreateAsync(Oferta entity)
        {
            _db.Set<Oferta>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Oferta entity)
        {
            _db.Set<Oferta>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Oferta>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Oferta>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
