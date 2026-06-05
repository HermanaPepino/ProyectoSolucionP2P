using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class ComprobantePagoRepository : IComprobantePagoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public ComprobantePagoRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<ComprobantePago> CreateAsync(ComprobantePago entity)
        {
            _db.ComprobantePago.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.ComprobantePago.FindAsync(id);
            if (ent == null) return;
            _db.ComprobantePago.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ComprobantePago>> GetAllAsync()
        {
            return await _db.ComprobantePago.ToListAsync();
        }

        public async Task<ComprobantePago?> GetByIdAsync(int id)
        {
            return await _db.ComprobantePago.FindAsync(id);
        }

        public async Task UpdateAsync(ComprobantePago entity)
        {
            _db.ComprobantePago.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
