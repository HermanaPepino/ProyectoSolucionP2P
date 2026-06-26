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
        {
            return await _db.Set<VerificacionIdentidad>()
            .Include(v => v.Usuario)
            .ToListAsync();
        }
        public async Task<IEnumerable<VerificacionIdentidad>> GetPendientesAsync()
        {
            return await _db.Set<VerificacionIdentidad>()
                .Include(v => v.Usuario)
                .Where(v => v.EstadoVerificacion == "Pendiente")
                .ToListAsync();
        }
        public async Task<VerificacionIdentidad?> GetByIdAsync(int id)
        {
            return await _db.Set<VerificacionIdentidad>()
            .Include(v => v.Usuario)
            .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VerificacionIdentidad?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _db.Set<VerificacionIdentidad>()
                .FirstOrDefaultAsync(v => v.UsuarioId == usuarioId);
        }

        public async Task<VerificacionIdentidad> CreateAsync(VerificacionIdentidad entity)
        {
            _db.Set<VerificacionIdentidad>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(VerificacionIdentidad entity)
        {
            _db.Entry(entity).State = EntityState.Modified;

            if (entity.Usuario != null)
            {
                _db.Entry(entity.Usuario).State = EntityState.Modified;
            }

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
