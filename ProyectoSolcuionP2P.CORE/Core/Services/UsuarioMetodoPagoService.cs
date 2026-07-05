using System.Text.Json;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class UsuarioMetodoPagoService : IUsuarioMetodoPagoService
    {
        private readonly IUsuarioMetodoPagoRepository _repo;

        public UsuarioMetodoPagoService(IUsuarioMetodoPagoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UsuarioMetodoPagoDto>> GetByUsuarioIdAsync(int usuarioId)
        {
            var lista = await _repo.GetByUsuarioIdAsync(usuarioId);
            return lista.Select(MapToDto);
        }

        public async Task<UsuarioMetodoPagoDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<UsuarioMetodoPagoDto> CreateAsync(UsuarioMetodoPagoCreateDto dto)
        {
            if (dto.UsuarioId <= 0)
                throw new InvalidOperationException("Usuario inválido.");

            if (dto.MetodoPagoId <= 0)
                throw new InvalidOperationException("Método de pago inválido.");

            if (string.IsNullOrWhiteSpace(dto.Alias))
                throw new InvalidOperationException("El alias es obligatorio.");

            if (dto.DatosPago == null || dto.DatosPago.Count == 0)
                throw new InvalidOperationException("Los datos del método de pago son obligatorios.");

            var entity = new UsuarioMetodoPago
            {
                UsuarioId = dto.UsuarioId,
                MetodoPagoId = dto.MetodoPagoId,
                Alias = dto.Alias.Trim(),
                DatosPago = JsonSerializer.Serialize(dto.DatosPago),
                ResumenPublico = string.IsNullOrWhiteSpace(dto.ResumenPublico)
                    ? CrearResumen(dto.Alias, dto.DatosPago)
                    : dto.ResumenPublico.Trim(),
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            var creado = await _repo.CreateAsync(entity);
            var completo = await _repo.GetByIdAsync(creado.Id);

            return MapToDto(completo!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        private static string CrearResumen(string alias, Dictionary<string, string> datos)
        {
            var valor = datos.Values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? alias;

            if (valor.Length <= 4)
                return $"{alias} • {valor}";

            return $"{alias} • ****{valor[^4..]}";
        }

        private static UsuarioMetodoPagoDto MapToDto(UsuarioMetodoPago e)
        {
            return new UsuarioMetodoPagoDto
            {
                Id = e.Id,
                UsuarioId = e.UsuarioId,
                MetodoPagoId = e.MetodoPagoId,
                MetodoPagoNombre = e.MetodoPago?.Nombre ?? string.Empty,
                Alias = e.Alias,
                ResumenPublico = e.ResumenPublico,
                Activo = e.Activo,
                FechaCreacion = e.FechaCreacion
            };
        }
    }
}