using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public MonedaRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Moneda>> GetAllAsync()
            => await _db.Set<Moneda>().AsNoTracking().ToListAsync();

        public async Task<Moneda?> GetByIdAsync(int id)
            => await _db.Set<Moneda>().FindAsync(id);

        public async Task<Moneda> CreateAsync(Moneda entity)
        {
            _db.Set<Moneda>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Moneda entity)
        {
            _db.Set<Moneda>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Moneda>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Moneda>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}