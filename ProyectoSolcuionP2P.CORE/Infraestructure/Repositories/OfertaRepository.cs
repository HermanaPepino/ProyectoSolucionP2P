using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public OfertaRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Oferta> CreateAsync(Oferta entity)
        {
            _db.Oferta.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Oferta.FindAsync(id);
            if (ent == null) return;
            _db.Oferta.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Oferta>> GetAllAsync()
        {
            return await _db.Oferta.ToListAsync();
        }

        public async Task<Oferta?> GetByIdAsync(int id)
        {
            return await _db.Oferta.FindAsync(id);
        }

        public async Task UpdateAsync(Oferta entity)
        {
            _db.Oferta.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
