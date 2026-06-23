using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class VerificacionIdentidadRepository : IVerificacionIdentidadRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public VerificacionIdentidadRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<VerificacionIdentidad>> GetAllAsync()
            => await _db.Set<VerificacionIdentidad>().AsNoTracking().ToListAsync();

        public async Task<VerificacionIdentidad?> GetByIdAsync(int id)
            => await _db.Set<VerificacionIdentidad>().FindAsync(id);

        public async Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity)
        {
            _db.Set<VerificacionIdentidad>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(VerificacionIdentidad entity)
        {
            _db.Set<VerificacionIdentidad>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<VerificacionIdentidad>().FindAsync(id);
            if (ent == null) return;
            _db.Set<VerificacionIdentidad>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
