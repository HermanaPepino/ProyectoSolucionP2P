using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class VerificacionIdentidadRepository : IVerificacionIdentidadRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public VerificacionIdentidadRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity)
        {
            _db.VerificacionIdentidad.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.VerificacionIdentidad.FindAsync(id);
            if (ent == null) return;
            _db.VerificacionIdentidad.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<VerificacionIdentidad>> GetAllAsync()
        {
            return await _db.VerificacionIdentidad.ToListAsync();
        }

        public async Task<VerificacionIdentidad?> GetByIdAsync(int id)
        {
            return await _db.VerificacionIdentidad.FindAsync(id);
        }

        public async Task UpdateAsync(VerificacionIdentidad entity)
        {
            _db.VerificacionIdentidad.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
