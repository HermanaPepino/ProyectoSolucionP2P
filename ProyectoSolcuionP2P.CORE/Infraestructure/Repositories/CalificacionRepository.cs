using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class CalificacionRepository : ICalificacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public CalificacionRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Calificacion> CreateAsync(Calificacion entity)
        {
            _db.Calificacion.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Calificacion.FindAsync(id);
            if (ent == null) return;
            _db.Calificacion.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Calificacion>> GetAllAsync()
        {
            return await _db.Calificacion.ToListAsync();
        }

        public async Task<Calificacion?> GetByIdAsync(int id)
        {
            return await _db.Calificacion.FindAsync(id);
        }

        public async Task UpdateAsync(Calificacion entity)
        {
            _db.Calificacion.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
