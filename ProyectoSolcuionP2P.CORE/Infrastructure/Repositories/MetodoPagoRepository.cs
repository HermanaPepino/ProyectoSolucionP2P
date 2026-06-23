using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public MetodoPagoRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<MetodoPago>> GetAllAsync()
            => await _db.Set<MetodoPago>().AsNoTracking().ToListAsync();

        public async Task<MetodoPago?> GetByIdAsync(int id)
            => await _db.Set<MetodoPago>().FindAsync(id);

        public async Task<MetodoPago> CreateAsync(MetodoPago entity)
        {
            _db.Set<MetodoPago>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(MetodoPago entity)
        {
            _db.Set<MetodoPago>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<MetodoPago>().FindAsync(id);
            if (ent == null) return;
            _db.Set<MetodoPago>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
