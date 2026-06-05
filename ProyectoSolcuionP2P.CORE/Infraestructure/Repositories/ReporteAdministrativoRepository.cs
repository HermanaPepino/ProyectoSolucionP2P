using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class ReporteAdministrativoRepository : IReporteAdministrativoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public ReporteAdministrativoRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<ReporteAdministrativo> CreateAsync(ReporteAdministrativo entity)
        {
            _db.ReporteAdministrativo.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.ReporteAdministrativo.FindAsync(id);
            if (ent == null) return;
            _db.ReporteAdministrativo.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReporteAdministrativo>> GetAllAsync()
        {
            return await _db.ReporteAdministrativo.ToListAsync();
        }

        public async Task<ReporteAdministrativo?> GetByIdAsync(int id)
        {
            return await _db.ReporteAdministrativo.FindAsync(id);
        }

        public async Task UpdateAsync(ReporteAdministrativo entity)
        {
            _db.ReporteAdministrativo.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
