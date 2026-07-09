using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ICalificacionRepository _repo;
        private readonly IOperacionRepository _operacionRepo;

        public CalificacionService(ICalificacionRepository repo, IOperacionRepository operacionRepo)
        {
            _repo = repo;
            _operacionRepo = operacionRepo;
        }

        public async Task<IEnumerable<CalificacionDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<CalificacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        // Se mantiene para no romper código existente, pero el controlador usa la versión
        // que recibe usuarioActualId para validar que el usuario califique solo a su contraparte.
        public async Task<CalificacionDto> CreateAsync(CalificacionDto dto)
        {
            ValidarDatosBasicos(dto);

            var e = new Calificacion
            {
                OperacionId = dto.OperacionId,
                UsuarioCalificadoId = dto.UsuarioCalificadoId,
                Puntaje = dto.Puntaje,
                Comentario = NormalizarComentario(dto.Comentario),
                FechaRegistro = dto.FechaRegistro ?? DateTime.Now
            };

            var creado = await _repo.CreateAsync(e);
            return MapToDto(creado);
        }

        public async Task<CalificacionDto> CreateAsync(CalificacionDto dto, int usuarioActualId)
        {
            ValidarDatosBasicos(dto);

            var operacion = await _operacionRepo.GetByIdAsync(dto.OperacionId);
            if (operacion == null)
                throw new InvalidOperationException("La operación no existe.");

            if (operacion.Estado != "Completada")
                throw new InvalidOperationException("Solo se pueden calificar operaciones completadas.");

            var esComprador = operacion.CompradorId == usuarioActualId;
            var esVendedor = operacion.VendedorId == usuarioActualId;

            if (!esComprador && !esVendedor)
                throw new InvalidOperationException("No tienes permiso para calificar esta operación.");

            var contraparteId = esComprador ? operacion.VendedorId : operacion.CompradorId;

            if (dto.UsuarioCalificadoId != contraparteId)
                throw new InvalidOperationException("Solo puedes calificar a la contraparte de la operación.");

            if (dto.UsuarioCalificadoId == usuarioActualId)
                throw new InvalidOperationException("No puedes calificarte a ti mismo.");

            var calificaciones = await _repo.GetAllAsync();
            var yaExiste = calificaciones.Any(c =>
                c.OperacionId == dto.OperacionId &&
                c.UsuarioCalificadoId == dto.UsuarioCalificadoId);

            if (yaExiste)
                throw new InvalidOperationException("Ya existe una calificación para esta contraparte en esta operación.");

            return await CreateAsync(dto);
        }

        public async Task<IEnumerable<CalificacionHistorialDto>> GetMisCalificacionesAsync(int usuarioActualId, int dias = 30)
        {
            var desde = DateTime.Now.AddDays(-Math.Max(1, dias));
            var calificaciones = await _repo.GetAllAsync();

            return calificaciones
                .Where(c => c.FechaRegistro == null || c.FechaRegistro >= desde)
                .Where(c => ParticipaUsuario(c, usuarioActualId))
                .Select(c => MapToHistorialDto(c, usuarioActualId))
                .OrderByDescending(c => c.FechaRegistro)
                .ToList();
        }

        public async Task<bool> UpdateAsync(int id, CalificacionDto dto)
        {
            ValidarDatosBasicos(dto);

            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            e.OperacionId = dto.OperacionId;
            e.UsuarioCalificadoId = dto.UsuarioCalificadoId;
            e.Puntaje = dto.Puntaje;
            e.Comentario = NormalizarComentario(dto.Comentario);
            e.FechaRegistro = dto.FechaRegistro ?? e.FechaRegistro;

            await _repo.UpdateAsync(e);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        private static bool ParticipaUsuario(Calificacion c, int usuarioId)
        {
            if (c.Operacion == null) return false;

            return c.UsuarioCalificadoId == usuarioId
                || c.Operacion.CompradorId == usuarioId
                || c.Operacion.VendedorId == usuarioId;
        }

        private static CalificacionHistorialDto MapToHistorialDto(Calificacion c, int usuarioActualId)
        {
            var operacion = c.Operacion;
            var usuarioCalificadorId = 0;
            var usuarioCalificadorNombre = string.Empty;

            if (operacion != null)
            {
                if (c.UsuarioCalificadoId == operacion.CompradorId)
                {
                    usuarioCalificadorId = operacion.VendedorId;
                    usuarioCalificadorNombre = operacion.Vendedor?.NombreCompleto ?? $"Usuario #{operacion.VendedorId}";
                }
                else
                {
                    usuarioCalificadorId = operacion.CompradorId;
                    usuarioCalificadorNombre = operacion.Comprador?.NombreCompleto ?? $"Usuario #{operacion.CompradorId}";
                }
            }

            return new CalificacionHistorialDto
            {
                Id = c.Id,
                OperacionId = c.OperacionId,
                CodigoOperacion = operacion?.CodigoOperacion ?? string.Empty,
                Tipo = c.UsuarioCalificadoId == usuarioActualId ? "Recibida" : "Dada",
                Puntaje = c.Puntaje,
                Comentario = c.Comentario,
                FechaRegistro = c.FechaRegistro,
                UsuarioCalificadoId = c.UsuarioCalificadoId,
                UsuarioCalificadoNombre = c.UsuarioCalificado?.NombreCompleto ?? $"Usuario #{c.UsuarioCalificadoId}",
                UsuarioCalificadorId = usuarioCalificadorId,
                UsuarioCalificadorNombre = usuarioCalificadorNombre,
                Monto = operacion?.Monto ?? 0,
                EstadoOperacion = operacion?.Estado ?? string.Empty
            };
        }

        private static void ValidarDatosBasicos(CalificacionDto dto)
        {
            if (dto.OperacionId <= 0)
                throw new InvalidOperationException("La operación es obligatoria.");

            if (dto.UsuarioCalificadoId <= 0)
                throw new InvalidOperationException("El usuario calificado es obligatorio.");

            if (dto.Puntaje < 1 || dto.Puntaje > 5)
                throw new InvalidOperationException("El puntaje debe estar entre 1 y 5.");

            if (!string.IsNullOrWhiteSpace(dto.Comentario) && dto.Comentario.Trim().Length > 300)
                throw new InvalidOperationException("El comentario no puede superar los 300 caracteres.");
        }

        private static string? NormalizarComentario(string? comentario)
        {
            var limpio = comentario?.Trim();
            return string.IsNullOrWhiteSpace(limpio) ? null : limpio;
        }

        private static CalificacionDto MapToDto(Calificacion e) => new CalificacionDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            UsuarioCalificadoId = e.UsuarioCalificadoId,
            Puntaje = e.Puntaje,
            Comentario = e.Comentario,
            FechaRegistro = e.FechaRegistro
        };
    }
}
