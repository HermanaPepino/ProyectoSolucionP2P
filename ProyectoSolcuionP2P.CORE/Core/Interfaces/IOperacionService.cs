using ProyectoSolucionP2P.CORE.Core.DTOs;

namespace ProyectoSolucionP2P.CORE.Core.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<OperacionDto>> GetAllAsync();
        Task<OperacionDto?> GetByIdAsync(int id);
        Task<OperacionDto> CreateAsync(OperacionDto dto);
        Task<bool> UpdateAsync(int id, OperacionDto dto);
        Task<bool> DeleteAsync(int id);

        // HU-008 / HU-009: Iniciar trato P2P + temporizador de seguridad
        Task<(OperacionDto? operacion, string? error)> IniciarTratoAsync(IniciarTratoDto dto, int compradorId);
        Task<(bool ok, string? error)> CancelarAsync(int operacionId, int usuarioId);

        // HU-010 / HU-011 / HU-012: pago enviado -> confirmación -> liberación
        Task<(bool ok, string? error)> MarcarPagoEnviadoAsync(int operacionId, int usuarioId);
        Task<(bool ok, string? error)> ConfirmarRecepcionPagoAsync(int operacionId, int usuarioId);
    }
}