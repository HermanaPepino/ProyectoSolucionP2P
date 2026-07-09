using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OfertaMetodoPagoService : IOfertaMetodoPagoService
    {
        private readonly IOfertaMetodoPagoRepository _repo;
        public OfertaMetodoPagoService(IOfertaMetodoPagoRepository repo) { _repo = repo; }

        public async Task<IEnumerable<OfertaMetodoPagoDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<OfertaMetodoPagoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<OfertaMetodoPagoDto> CreateAsync(OfertaMetodoPagoDto dto)
        {
            var e = new OfertaMetodoPago
            {
                OfertaId = dto.OfertaId,
                MetodoPagoId = dto.MetodoPagoId,
                Alias = dto.Alias,
                DatosRecepcion = dto.DatosRecepcion,
                Instrucciones = dto.Instrucciones,
                ResumenPublico = dto.ResumenPublico
            };
            var creado = await _repo.CreateAsync(e);

            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, OfertaMetodoPagoDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            e.OfertaId = dto.OfertaId;
            e.MetodoPagoId = dto.MetodoPagoId;
            e.Alias = dto.Alias;
            e.DatosRecepcion = dto.DatosRecepcion;
            e.Instrucciones = dto.Instrucciones;
            e.ResumenPublico = dto.ResumenPublico;

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

        private static OfertaMetodoPagoDto MapToDto(OfertaMetodoPago e) => new OfertaMetodoPagoDto
        {
            Id = e.Id,
            OfertaId = e.OfertaId,
            MetodoPagoId = e.MetodoPagoId,
            MetodoPagoNombre = e.MetodoPago?.Nombre ?? string.Empty,
            Alias = e.Alias,
            DatosRecepcion = e.DatosRecepcion,
            Instrucciones = e.Instrucciones,
            ResumenPublico = e.ResumenPublico
        };
    }
}
