using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public UsuarioRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
            => await _db.Usuarios.AsNoTracking().ToListAsync();

        public async Task<Usuario?> GetByIdAsync(int id)
            => await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Usuario?> GetByCorreoAsync(string correo)
            => await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

        public async Task<Usuario> CreateAsync(Usuario entity)
        {
            _db.Usuarios.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _db.Usuarios.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Usuarios.FindAsync(id);
            if (ent == null) return;
            _db.Usuarios.Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}