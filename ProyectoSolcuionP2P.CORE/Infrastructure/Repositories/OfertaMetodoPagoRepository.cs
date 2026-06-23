using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class OfertaMetodoPagoRepository : IOfertaMetodoPagoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public OfertaMetodoPagoRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<OfertaMetodoPago>> GetAllAsync()
            => await _db.Set<OfertaMetodoPago>().AsNoTracking().ToListAsync();

        public async Task<OfertaMetodoPago?> GetByIdAsync(int id)
            => await _db.Set<OfertaMetodoPago>().FindAsync(id);

        public async Task<OfertaMetodoPago> CreateAsync(OfertaMetodoPago entity)
        {
            _db.Set<OfertaMetodoPago>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(OfertaMetodoPago entity)
        {
            _db.Set<OfertaMetodoPago>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<OfertaMetodoPago>().FindAsync(id);
            if (ent == null) return;
            _db.Set<OfertaMetodoPago>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
