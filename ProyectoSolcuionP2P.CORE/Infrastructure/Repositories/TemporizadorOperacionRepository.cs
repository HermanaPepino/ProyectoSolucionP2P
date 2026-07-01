using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class TemporizadorOperacionRepository : ITemporizadorOperacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public TemporizadorOperacionRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<TemporizadorOperacion>> GetAllAsync()
            => await _db.Set<TemporizadorOperacion>().AsNoTracking().ToListAsync();

        public async Task<TemporizadorOperacion?> GetByIdAsync(int id)
            => await _db.Set<TemporizadorOperacion>().FindAsync(id);

        public async Task<TemporizadorOperacion?> GetByOperacionIdAsync(int operacionId)
            => await _db.Set<TemporizadorOperacion>()
                .Where(t => t.OperacionId == operacionId)
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

        public async Task<TemporizadorOperacion> CreateAsync(TemporizadorOperacion entity)
        {
            _db.Set<TemporizadorOperacion>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TemporizadorOperacion entity)
        {
            _db.Set<TemporizadorOperacion>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<TemporizadorOperacion>().FindAsync(id);
            if (ent == null) return;
            _db.Set<TemporizadorOperacion>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}