using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class UsuarioMetodoPagoRepository : IUsuarioMetodoPagoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public UsuarioMetodoPagoRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UsuarioMetodoPago>> GetByUsuarioIdAsync(int usuarioId)
            => await _db.UsuarioMetodoPagos
                .Include(x => x.MetodoPago)
                .Where(x => x.UsuarioId == usuarioId && x.Activo)
                .AsNoTracking()
                .ToListAsync();

        public async Task<UsuarioMetodoPago?> GetByIdAsync(int id)
            => await _db.UsuarioMetodoPagos
                .Include(x => x.MetodoPago)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<UsuarioMetodoPago> CreateAsync(UsuarioMetodoPago entity)
        {
            _db.UsuarioMetodoPagos.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(UsuarioMetodoPago entity)
        {
            _db.UsuarioMetodoPagos.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.UsuarioMetodoPagos.FindAsync(id);
            if (entity == null) return;

            entity.Activo = false;
            await _db.SaveChangesAsync();
        }
    }
}