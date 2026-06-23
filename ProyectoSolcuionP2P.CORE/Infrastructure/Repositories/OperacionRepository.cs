using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class OperacionRepository : IOperacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public OperacionRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Operacion>> GetAllAsync()
            => await _db.Set<Operacion>()
                .Include(o => o.Comprador)
                .Include(o => o.Vendedor)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Operacion?> GetByIdAsync(int id)
            => await _db.Set<Operacion>()
                .Include(o => o.Comprador)
                .Include(o => o.Vendedor)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Operacion> CreateAsync(Operacion entity)
        {
            _db.Set<Operacion>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Operacion entity)
        {
            _db.Set<Operacion>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Operacion>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Operacion>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
