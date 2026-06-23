using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class MensajeService : IMensajeService
    {
        private readonly IMensajeRepository _repo;
        public MensajeService(IMensajeRepository repo) { _repo = repo; }

        public async Task<IEnumerable<MensajeDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<MensajeDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<MensajeDto> CreateAsync(MensajeDto dto)
        {
            var e = new Mensaje 
            { 
                RemitenteId = dto.RemitenteId,
                DestinatarioId = dto.DestinatarioId,
                OperacionId = dto.OperacionId,
                Contenido = dto.Contenido,
                FechaEnvio = dto.FechaEnvio
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, MensajeDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.RemitenteId = dto.RemitenteId;
            e.DestinatarioId = dto.DestinatarioId;
            e.OperacionId = dto.OperacionId;
            e.Contenido = dto.Contenido;
            e.FechaEnvio = dto.FechaEnvio;
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

        private static MensajeDto MapToDto(Mensaje e) => new MensajeDto
        {
            Id = e.Id,
            RemitenteId = e.RemitenteId,
            DestinatarioId = e.DestinatarioId,
            OperacionId = e.OperacionId,
            RemitenteNombre = e.Remitente?.NombreCompleto ?? string.Empty,
            DestinatarioNombre = e.Destinatario?.NombreCompleto ?? string.Empty,
            Contenido = e.Contenido,
            FechaEnvio = e.FechaEnvio
        };
    }
}
