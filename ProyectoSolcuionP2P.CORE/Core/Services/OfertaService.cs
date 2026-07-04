using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository _repo;
        public OfertaService(IOfertaRepository repo) { _repo = repo; }

        public async Task<IEnumerable<OfertaDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<OfertaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<OfertaDto> CreateAsync(OfertaDto dto)
        {
            var e = new Oferta 
            { 
                UsuarioId = dto.UsuarioId,
                MonedaOrigenId = dto.MonedaOrigenId,
                MonedaDestinoId = dto.MonedaDestinoId,
                TipoOperacion = dto.TipoOperacion,
                TasaCambio = dto.TasaCambio,
                MontoMinimo = dto.MontoMinimo,
                MontoMaximo = dto.MontoMaximo,
                MontoDisponible = dto.MontoDisponible,
                Estado = dto.Estado,
                FechaCreacion = dto.FechaCreacion
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, OfertaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.UsuarioId = dto.UsuarioId;
            e.MonedaOrigenId = dto.MonedaOrigenId;
            e.MonedaDestinoId = dto.MonedaDestinoId;
            e.TipoOperacion = dto.TipoOperacion;
            e.TasaCambio = dto.TasaCambio;
            e.MontoMinimo = dto.MontoMinimo;
            e.MontoMaximo = dto.MontoMaximo;
            e.MontoDisponible = dto.MontoDisponible;
            e.Estado = dto.Estado;
            e.FechaCreacion = dto.FechaCreacion;
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

        private static OfertaDto MapToDto(Oferta e) => new OfertaDto
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            NombreVendedor = e.Usuario?.NombreCompleto,
            MonedaOrigenId = e.MonedaOrigenId,
            MonedaDestinoId = e.MonedaDestinoId,
            MonedaOrigenNombre = e.MonedaOrigen?.Nombre ?? string.Empty,
            MonedaDestinoNombre = e.MonedaDestino?.Nombre ?? string.Empty,
            TipoOperacion = e.TipoOperacion,
            TasaCambio = e.TasaCambio,
            MontoMinimo = e.MontoMinimo,
            MontoMaximo = e.MontoMaximo,
            MontoDisponible = e.MontoDisponible,
            Estado = e.Estado,
            FechaCreacion = e.FechaCreacion
        };
    }
}
