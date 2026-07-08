using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Repositories
{
    public class ReputacionRepository : IReputacionRepository
    {
        private readonly CambioSeguroP2pdbContext _db;

        public ReputacionRepository(CambioSeguroP2pdbContext db)
        {
            _db = db;
        }

        public async Task<UsuarioReputacionDto> ObtenerReputacionAsync(int usuarioId)
        {
            var usuario = await _db.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == usuarioId);

            if (usuario == null)
                return null!;

            var calificaciones = await _db.Calificacions
                .AsNoTracking()
                .Where(c => c.UsuarioCalificadoId == usuarioId)
                .ToListAsync();

            var promedio = calificaciones.Any()
                ? Math.Round((decimal)calificaciones.Average(c => c.Puntaje), 2)
                : 0;

            var operaciones = await _db.Operacions
                .AsNoTracking()
                .Where(o => o.VendedorId == usuarioId || o.CompradorId == usuarioId)
                .ToListAsync();

            var operacionesCompletadas = operaciones
                .Count(o => o.Estado == "Completada");

            // Solo contamos estados finales. Si una operación sigue en proceso, pago enviado
            // o disputa abierta, todavía no debería perjudicar la tasa de éxito del usuario.
            var estadosFinales = new[] { "Completada", "Cancelada", "Expirada" };

            var operacionesFinalizadas = operaciones
                .Count(o => estadosFinales.Contains(o.Estado));

            var tasaExito = operacionesFinalizadas == 0
                ? 0
                : Math.Round((decimal)operacionesCompletadas * 100 / operacionesFinalizadas, 2);

            var operacionesConTiempo = operaciones
                .Where(o =>
                    o.Estado == "Completada" &&
                    o.FechaInicio.HasValue &&
                    o.FechaFin.HasValue &&
                    o.FechaFin.Value >= o.FechaInicio.Value)
                .ToList();

            var tiempoPromedioOperacionMinutos = operacionesConTiempo.Any()
                ? Math.Round((decimal)operacionesConTiempo
                    .Average(o => (o.FechaFin!.Value - o.FechaInicio!.Value).TotalMinutes), 2)
                : 0;

            return new UsuarioReputacionDto
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.NombreCompleto,
                CalificacionPromedio = promedio,
                CantidadCalificaciones = calificaciones.Count,
                OperacionesCompletadas = operacionesCompletadas,
                OperacionesFinalizadas = operacionesFinalizadas,
                TasaExito = tasaExito,
                TiempoPromedioOperacionMinutos = tiempoPromedioOperacionMinutos,
                Comentarios = calificaciones
                    .OrderByDescending(c => c.FechaRegistro)
                    .Select(c => c.Comentario?.Trim())
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Take(5)
                    .Select(c => c!)
                    .ToList()
            };
        }
    }
}
