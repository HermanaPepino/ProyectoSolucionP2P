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
                .FirstOrDefaultAsync(x => x.Id == usuarioId);

            if (usuario == null)
                return null!;

            var calificaciones = await _db.Calificacions
                .Where(c => c.UsuarioCalificadoId == usuarioId)
                .ToListAsync();

            var promedio = calificaciones.Any()
                ? Math.Round(calificaciones.Average(c => c.Puntaje), 2)
                : 0;
            // Todas las operaciones donde participó el usuario
            var operaciones = await _db.Operacions
                .Where(o => o.VendedorId == usuarioId || o.CompradorId == usuarioId)
                .ToListAsync();

            // Solo las operaciones completadas
            var operacionesCompletadas = operaciones.Count(o => o.Estado == "Completada");

            // Tasa de éxito
            decimal tasaExito = operaciones.Count == 0
                ? 0
                : Math.Round((decimal)operacionesCompletadas * 100 / operaciones.Count, 2);

            return new UsuarioReputacionDto
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.NombreCompleto,

                CalificacionPromedio = (decimal)promedio,

                CantidadCalificaciones = calificaciones.Count,

                OperacionesCompletadas = operacionesCompletadas,

                TasaExito = tasaExito,

                Comentarios = calificaciones
                    .OrderByDescending(c => c.FechaRegistro)
                    .Take(5)
                    .Select(c => c.Comentario ?? "")
                    .ToList()
            };
        }
    }
}