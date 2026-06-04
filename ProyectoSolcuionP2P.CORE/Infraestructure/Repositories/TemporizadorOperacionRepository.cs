using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class TemporizadorOperacionRepository : ITemporizadorOperacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public TemporizadorOperacionRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<TemporizadorOperacion> CreateAsync(TemporizadorOperacion entity)
        {
            _db.TemporizadorOperacion.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.TemporizadorOperacion.FindAsync(id);
            if (ent == null) return;
            _db.TemporizadorOperacion.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TemporizadorOperacion>> GetAllAsync()
        {
            return await _db.TemporizadorOperacion.ToListAsync();
        }

        public async Task<TemporizadorOperacion?> GetByIdAsync(int id)
        {
            return await _db.TemporizadorOperacion.FindAsync(id);
        }

        public async Task UpdateAsync(TemporizadorOperacion entity)
        {
            _db.TemporizadorOperacion.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
