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
    public class OfertaController : ControllerBase
    {
        private readonly IOfertaService _service;

        public OfertaController(IOfertaService service)
        {
            _service = service;
        }

        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfertaDto dto)
        {
            if (User.IsInRole("Administrador"))
                return BadRequest(new { mensaje = "El administrador gestiona la plataforma, pero no puede publicar ofertas como vendedor." });

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OfertaDto dto)
        {
            try
            {
                return await _service.UpdateAsync(id, dto)
                    ? NoContent()
                    : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}