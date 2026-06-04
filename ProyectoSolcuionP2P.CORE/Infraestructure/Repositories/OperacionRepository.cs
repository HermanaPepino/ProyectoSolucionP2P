using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class OperacionRepository : IOperacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public OperacionRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Operacion> CreateAsync(Operacion entity)
        {
            _db.Operacion.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Operacion.FindAsync(id);
            if (ent == null) return;
            _db.Operacion.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Operacion>> GetAllAsync()
        {
            return await _db.Operacion.ToListAsync();
        }

        public async Task<Operacion?> GetByIdAsync(int id)
        {
            return await _db.Operacion.FindAsync(id);
        }

        public async Task UpdateAsync(Operacion entity)
        {
            _db.Operacion.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
