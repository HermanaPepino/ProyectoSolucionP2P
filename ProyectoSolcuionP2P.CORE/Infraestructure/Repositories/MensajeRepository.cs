using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class MensajeRepository : IMensajeRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public MensajeRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Mensaje> CreateAsync(Mensaje entity)
        {
            _db.Mensaje.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Mensaje.FindAsync(id);
            if (ent == null) return;
            _db.Mensaje.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mensaje>> GetAllAsync()
        {
            return await _db.Mensaje.ToListAsync();
        }

        public async Task<Mensaje?> GetByIdAsync(int id)
        {
            return await _db.Mensaje.FindAsync(id);
        }

        public async Task UpdateAsync(Mensaje entity)
        {
            _db.Mensaje.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
