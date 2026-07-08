using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class DisputaRepository : IDisputaRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public DisputaRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        private IQueryable<Disputa> QueryCompleto()
            => _db.Set<Disputa>()
                .Include(d => d.EvidenciaDisputa)
                .Include(d => d.Operacion)
                    .ThenInclude(o => o.Comprador)
                .Include(d => d.Operacion)
                    .ThenInclude(o => o.Vendedor)
                .Include(d => d.Operacion)
                    .ThenInclude(o => o.MetodoPago)
                .Include(d => d.Operacion)
                    .ThenInclude(o => o.OfertaMetodoPago)
                        .ThenInclude(omp => omp.MetodoPago);

        public async Task<IEnumerable<Disputa>> GetAllAsync()
            => await QueryCompleto()
                .AsNoTracking()
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();

        public async Task<Disputa?> GetByIdAsync(int id)
            => await QueryCompleto()
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<Disputa?> GetByOperacionIdAsync(int operacionId)
            => await QueryCompleto()
                .AsNoTracking()
                .Where(d => d.OperacionId == operacionId)
                .OrderByDescending(d => d.FechaRegistro)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Disputa>> GetByUsuarioIdAsync(int usuarioId)
            => await QueryCompleto()
                .AsNoTracking()
                .Where(d => d.Operacion.CompradorId == usuarioId || d.Operacion.VendedorId == usuarioId)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();

        public async Task<Disputa> CreateAsync(Disputa entity)
        {
            _db.Set<Disputa>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Disputa entity)
        {
            _db.Set<Disputa>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<Disputa>().FindAsync(id);
            if (ent == null) return;

            _db.Set<Disputa>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
