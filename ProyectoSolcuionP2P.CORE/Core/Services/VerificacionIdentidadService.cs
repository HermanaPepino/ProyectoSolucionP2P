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

        public async Task<VerificacionIdentidadDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<VerificacionIdentidadDto> CreateAsync(VerificacionIdentidadDto dto)
        {
            var e = new VerificacionIdentidad 
            { 
                UsuarioId = dto.UsuarioId,
                DocumentoIdentidad = dto.DocumentoIdentidad,
                TipoDocumento = dto.TipoDocumento,
                EstadoVerificacion = dto.EstadoVerificacion,
                FechaRegistro = dto.FechaRegistro
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
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

        private static VerificacionIdentidadDto MapToDto(VerificacionIdentidad e) => new VerificacionIdentidadDto
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            DocumentoIdentidad = e.DocumentoIdentidad,
            TipoDocumento = e.TipoDocumento,
            EstadoVerificacion = e.EstadoVerificacion,
            FechaRegistro = e.FechaRegistro
        };
    }
}
