using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class ReporteAdministrativoService : IReporteAdministrativoService
    {
        private readonly IReporteAdministrativoRepository _repo;
        public ReporteAdministrativoService(IReporteAdministrativoRepository repo) { _repo = repo; }

        public async Task<IEnumerable<ReporteAdministrativoDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<ReporteAdministrativoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }
        public async Task<DashboardAdministrativoDto> ObtenerDashboardAsync()
        {
            return await _repo.ObtenerDashboardAsync();
        }
        public async Task<ReporteAdministrativoDto> CreateAsync(ReporteAdministrativoDto dto)
        {
            var e = new ReporteAdministrativo 
            { 
                GeneradoPorUsuarioId = dto.GeneradoPorUsuarioId,
                FechaGeneracion = dto.FechaGeneracion,
                TotalOperaciones = dto.TotalOperaciones,
                TotalDisputas = dto.TotalDisputas,
                TotalUsuarios = dto.TotalUsuarios
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, ReporteAdministrativoDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.GeneradoPorUsuarioId = dto.GeneradoPorUsuarioId;
            e.FechaGeneracion = dto.FechaGeneracion;
            e.TotalOperaciones = dto.TotalOperaciones;
            e.TotalDisputas = dto.TotalDisputas;
            e.TotalUsuarios = dto.TotalUsuarios;
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

        private static ReporteAdministrativoDto MapToDto(ReporteAdministrativo e) => new ReporteAdministrativoDto
        {
            Id = e.Id,
            GeneradoPorUsuarioId = e.GeneradoPorUsuarioId,
            FechaGeneracion = e.FechaGeneracion,
            TotalOperaciones = e.TotalOperaciones,
            TotalDisputas = e.TotalDisputas,
            TotalUsuarios = e.TotalUsuarios
        };
    }
}
