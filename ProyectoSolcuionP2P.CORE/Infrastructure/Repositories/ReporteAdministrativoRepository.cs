using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class ReporteAdministrativoRepository : IReporteAdministrativoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public ReporteAdministrativoRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<ReporteAdministrativo>> GetAllAsync()
            => await _db.Set<ReporteAdministrativo>().AsNoTracking().ToListAsync();

        public async Task<ReporteAdministrativo?> GetByIdAsync(int id)
            => await _db.Set<ReporteAdministrativo>().FindAsync(id);

        public async Task<ReporteAdministrativo> CreateAsync(ReporteAdministrativo entity)
        {
            _db.Set<ReporteAdministrativo>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ReporteAdministrativo entity)
        {
            _db.Set<ReporteAdministrativo>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<ReporteAdministrativo>().FindAsync(id);
            if (ent == null) return;
            _db.Set<ReporteAdministrativo>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
