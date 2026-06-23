using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public NotificacionRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<Notificacion>> GetAllAsync()
            => await _db.Set<Notificacion>().AsNoTracking().ToListAsync();

        public async Task<Notificacion?> GetByIdAsync(int id)
            => await _db.Set<Notificacion>().FindAsync(id);

        public async Task<Notificacion> CreateAsync(Notificacion entity)
        {
            _db.Set<Notificacion>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Notificacion entity)
        {
            _db.Set<Notificacion>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Notificacion>().FindAsync(id);
            if (ent == null) return;
            _db.Set<Notificacion>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
