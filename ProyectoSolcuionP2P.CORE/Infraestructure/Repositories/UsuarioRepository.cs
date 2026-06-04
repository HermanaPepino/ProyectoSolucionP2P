using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public UsuarioRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<Usuario> CreateAsync(Usuario entity)
        {
            _db.Usuario.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Usuario.FindAsync(id);
            if (ent == null) return;
            _db.Usuario.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _db.Usuario.ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _db.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _db.Usuario.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
