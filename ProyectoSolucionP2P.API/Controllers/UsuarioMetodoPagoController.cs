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
    public class UsuarioMetodoPagoController : ControllerBase
    {
        private readonly IUsuarioMetodoPagoService _service;

        public UsuarioMetodoPagoController(IUsuarioMetodoPagoService service)
        {
            _service = service;
        }

        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool EsAdmin =>
            User.IsInRole("Administrador");

        [HttpGet("mis-metodos")]
        public async Task<IActionResult> MisMetodos()
        {
            return Ok(await _service.GetByUsuarioIdAsync(UsuarioActualId));
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            if (!EsAdmin && usuarioId != UsuarioActualId)
                return Forbid();

            return Ok(await _service.GetByUsuarioIdAsync(usuarioId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            if (!EsAdmin && dto.UsuarioId != UsuarioActualId)
                return Forbid();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioMetodoPagoCreateDto dto)
        {
            try
            {
                dto.UsuarioId = UsuarioActualId;

                var creado = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            if (!EsAdmin && dto.UsuarioId != UsuarioActualId)
                return Forbid();

            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}