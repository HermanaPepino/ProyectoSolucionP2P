using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class DisputaRepository : IDisputaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public DisputaRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Disputa> CreateAsync(Disputa entity)
        {
            _db.Disputa.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Disputa.FindAsync(id);
            if (ent == null) return;
            _db.Disputa.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Disputa>> GetAllAsync()
        {
            return await _db.Disputa.ToListAsync();
        }

        public async Task<Disputa?> GetByIdAsync(int id)
        {
            return await _db.Disputa.FindAsync(id);
        }

        public async Task UpdateAsync(Disputa entity)
        {
            _db.Disputa.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
