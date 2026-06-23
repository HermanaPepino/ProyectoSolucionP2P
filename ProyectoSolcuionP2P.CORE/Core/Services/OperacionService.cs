using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OperacionService : IOperacionService
    {
        private readonly IOperacionRepository _repo;
        public OperacionService(IOperacionRepository repo) { _repo = repo; }

        public async Task<IEnumerable<OperacionDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<OperacionDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<OperacionDto> CreateAsync(OperacionDto dto)
        {
            var e = new Operacion 
            { 
                OfertaId = dto.OfertaId,
                CompradorId = dto.CompradorId,
                VendedorId = dto.VendedorId,
                Monto = dto.Monto,
                Estado = dto.Estado,
                CodigoOperacion = dto.CodigoOperacion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                FechaLiberacion = dto.FechaLiberacion
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, OperacionDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OfertaId = dto.OfertaId;
            e.CompradorId = dto.CompradorId;
            e.VendedorId = dto.VendedorId;
            e.Monto = dto.Monto;
            e.Estado = dto.Estado;
            e.CodigoOperacion = dto.CodigoOperacion;
            e.FechaInicio = dto.FechaInicio;
            e.FechaFin = dto.FechaFin;
            e.FechaLiberacion = dto.FechaLiberacion;
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

        private static OperacionDto MapToDto(Operacion e) => new OperacionDto
        {
            Id = e.Id,
            OfertaId = e.OfertaId,
            CompradorId = e.CompradorId,
            VendedorId = e.VendedorId,
            CompradorNombre = e.Comprador?.NombreCompleto ?? string.Empty,
            VendedorNombre = e.Vendedor?.NombreCompleto ?? string.Empty,
            Monto = e.Monto,
            Estado = e.Estado,
            CodigoOperacion = e.CodigoOperacion,
            FechaInicio = e.FechaInicio,
            FechaFin = e.FechaFin,
            FechaLiberacion = e.FechaLiberacion
        };
    }
}
