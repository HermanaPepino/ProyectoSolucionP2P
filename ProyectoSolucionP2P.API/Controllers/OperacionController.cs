using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OperacionController : ControllerBase
    {
        private readonly IOperacionService _service;
        public OperacionController(IOperacionService service) { _service = service; }

        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OperacionDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OperacionDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();

        // HU-008 / HU-009: el comprador reserva la oferta y arranca el temporizador de 15 min
        [HttpPost("iniciar-trato")]
        public async Task<IActionResult> IniciarTrato(IniciarTratoDto dto)
        {
            var (operacion, error) = await _service.IniciarTratoAsync(dto, UsuarioActualId);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = operacion!.Id }, operacion);
        }

        // Cancela una operación en proceso y libera la oferta
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var (ok, error) = await _service.CancelarAsync(id, UsuarioActualId);
            if (!ok) return BadRequest(new { mensaje = error });
            return NoContent();
        }

        // HU-011: el vendedor confirma que recibió el pago -> completa la operación
        [HttpPut("{id}/confirmar-recepcion")]
        public async Task<IActionResult> ConfirmarRecepcion(int id)
        {
            var (ok, error) = await _service.ConfirmarRecepcionPagoAsync(id, UsuarioActualId);
            if (!ok) return BadRequest(new { mensaje = error });
            return NoContent();
        }
    }
}