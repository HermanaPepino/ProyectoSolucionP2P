using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class VerificacionIdentidadService : IVerificacionIdentidadService
    {
        private readonly IVerificacionIdentidadRepository _repo;
        public VerificacionIdentidadService(IVerificacionIdentidadRepository repo) { _repo = repo; }

        public async Task<IEnumerable<VerificacionIdentidadDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);
        public async Task<IEnumerable<VerificacionIdentidadDto>> GetPendientesAsync()
        {
            var lista = await _repo.GetPendientesAsync();

            return lista.Select(MapToDto);
        }
        public async Task<VerificacionIdentidadDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<VerificacionIdentidadDto?> GetByUsuarioIdAsync(int usuarioId)
        {
            var entidad = await _repo.GetByUsuarioIdAsync(usuarioId);

            if (entidad == null)
                return null;

            return MapToDto(entidad);
        }

        public async Task<VerificacionIdentidadDto> CreateAsync(VerificacionIdentidadDto dto)
        {
            var existente = await _repo.GetByUsuarioIdAsync(dto.UsuarioId);

            if (existente != null)
            {
                if (existente.EstadoVerificacion == "Verificado")
                    return MapToDto(existente);

                existente.DocumentoIdentidad = dto.DocumentoIdentidad;
                existente.TipoDocumento = dto.TipoDocumento;
                existente.EstadoVerificacion = "Pendiente";

                await _repo.UpdateAsync(existente);

                return MapToDto(existente);
            }

            var nueva = new VerificacionIdentidad
            {
                UsuarioId = dto.UsuarioId,
                DocumentoIdentidad = dto.DocumentoIdentidad,
                TipoDocumento = dto.TipoDocumento,
                EstadoVerificacion = "Pendiente",
                FechaRegistro = DateTime.Now
            };

            var creada = await _repo.CreateAsync(nueva);

            return MapToDto(creada);


        }

        public async Task<bool> UpdateAsync(int id, VerificacionIdentidadDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.UsuarioId = dto.UsuarioId;
            e.DocumentoIdentidad = dto.DocumentoIdentidad;
            e.TipoDocumento = dto.TipoDocumento;
            e.EstadoVerificacion = dto.EstadoVerificacion;
            e.FechaRegistro = dto.FechaRegistro;
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

        private static VerificacionIdentidadDto MapToDto(VerificacionIdentidad e)
        {
            return new VerificacionIdentidadDto
            {
                Id = e.Id,
                UsuarioId = e.UsuarioId,
                DocumentoIdentidad = e.DocumentoIdentidad,
                TipoDocumento = e.TipoDocumento,
                EstadoVerificacion = e.EstadoVerificacion,
                FechaRegistro = e.FechaRegistro,

                NombreCompleto = e.Usuario?.NombreCompleto,
                Correo = e.Usuario?.Correo
            };
        }

        public async Task<bool> AprobarAsync(int id)
        {
            var verificacion = await _repo.GetByIdAsync(id);

            if (verificacion == null)
                return false;

            verificacion.EstadoVerificacion = "Verificado";

            verificacion.Usuario.EstadoVerificacion = "Verificado";

            await _repo.UpdateAsync(verificacion);

            return true;
        }

        public async Task<bool> RechazarAsync(int id)
        {
            var verificacion = await _repo.GetByIdAsync(id);

            if (verificacion == null)
                return false;

            verificacion.EstadoVerificacion = "Rechazado";

            verificacion.Usuario.EstadoVerificacion = "Rechazado";

            await _repo.UpdateAsync(verificacion);

            return true;
        }
    }
}
