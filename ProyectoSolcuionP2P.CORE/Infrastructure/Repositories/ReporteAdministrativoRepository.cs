using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class ReporteAdministrativoRepository : IReporteAdministrativoRepository
    {
        private readonly CambioSeguroP2pdbContext _db;
        public ReporteAdministrativoRepository(CambioSeguroP2pdbContext db) { _db = db; }

        public async Task<IEnumerable<ReporteAdministrativo>> GetAllAsync()
            => await _db.Set<ReporteAdministrativo>().AsNoTracking().ToListAsync();

        public async Task<ReporteAdministrativo?> GetByIdAsync(int id)
            => await _db.Set<ReporteAdministrativo>().FindAsync(id);

        public async Task<DashboardAdministrativoDto> ObtenerDashboardAsync()
        {
            var totalUsuarios = await _db.Set<Usuario>().CountAsync();
            var usuariosActivos = await _db.Set<Usuario>()
                .CountAsync(u => u.EstadoVerificacion == "Verificado");

            var totalOperaciones = await _db.Set<Operacion>().CountAsync();
            var operacionesCompletadas = await _db.Set<Operacion>()
                .CountAsync(o => o.Estado == "Completada");

            var totalDisputas = await _db.Set<Disputa>().CountAsync();
            var verificacionesPendientes = await _db.Set<VerificacionIdentidad>()
                .CountAsync(v => v.EstadoVerificacion == "Pendiente");

            var ofertasActivas = await _db.Set<Oferta>()
                .CountAsync(o => o.Estado == "Activa");

            var volumenIntercambio = await _db.Set<Operacion>()
                .Where(o => o.Estado == "Completada")
                .SumAsync(o => (decimal?)o.Monto) ?? 0m;

            var monedasMasUsadas = await ObtenerMonedasMasUsadasAsync(soloCompletadas: true);

            if (monedasMasUsadas.Count == 0)
                monedasMasUsadas = await ObtenerMonedasMasUsadasAsync(soloCompletadas: false);

            return new DashboardAdministrativoDto
            {
                TotalUsuarios = totalUsuarios,
                UsuariosActivos = usuariosActivos,
                TotalOperaciones = totalOperaciones,
                OperacionesCompletadas = operacionesCompletadas,
                TotalDisputas = totalDisputas,
                VerificacionesPendientes = verificacionesPendientes,
                OfertasActivas = ofertasActivas,
                VolumenIntercambio = volumenIntercambio,
                MonedaMasUsada = monedasMasUsadas.FirstOrDefault()?.Codigo ?? "—",
                MonedasMasUsadas = monedasMasUsadas
            };
        }

        private async Task<List<MonedaUsoDto>> ObtenerMonedasMasUsadasAsync(bool soloCompletadas)
        {
            var query = _db.Set<Operacion>().AsNoTracking();

            if (soloCompletadas)
                query = query.Where(o => o.Estado == "Completada");

            return await query
                .GroupBy(o => new
                {
                    o.Oferta.MonedaOrigen.Codigo,
                    o.Oferta.MonedaOrigen.Nombre
                })
                .Select(g => new MonedaUsoDto
                {
                    Codigo = g.Key.Codigo,
                    Nombre = g.Key.Nombre,
                    CantidadOperaciones = g.Count(),
                    Volumen = g.Sum(o => o.Monto)
                })
                .OrderByDescending(x => x.CantidadOperaciones)
                .ThenByDescending(x => x.Volumen)
                .Take(5)
                .ToListAsync();
        }

        public async Task<ReporteAdministrativo> CreateAsync(ReporteAdministrativo entity)
        {
            _db.Set<ReporteAdministrativo>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ReporteAdministrativo entity)
        {
            _db.Set<ReporteAdministrativo>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Set<ReporteAdministrativo>().FindAsync(id);
            if (ent == null) return;
            _db.Set<ReporteAdministrativo>().Remove(ent);
            await _db.SaveChangesAsync();
        }
    }
}
