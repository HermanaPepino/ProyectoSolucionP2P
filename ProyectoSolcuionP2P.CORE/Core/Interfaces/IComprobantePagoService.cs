using Microsoft.AspNetCore.Http;
using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IComprobantePagoService
    {
        Task<IEnumerable<ComprobantePagoDto>> GetAllAsync();
        Task<ComprobantePagoDto?> GetByIdAsync(int id);
        Task<ComprobantePagoDto?> GetByOperacionIdAsync(int operacionId);
        Task<ComprobantePagoDto> CreateAsync(ComprobantePagoDto dto);
        Task<bool> UpdateAsync(int id, ComprobantePagoDto dto);
        Task<bool> DeleteAsync(int id);

        // HU-010: sube el archivo real y marca la operación como "Pago enviado"
        Task<(ComprobantePagoDto? comprobante, string? error)> SubirAsync(int operacionId, int usuarioId, IFormFile archivo);
    }
}