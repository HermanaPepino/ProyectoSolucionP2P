using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificacionIdentidadController : ControllerBase
    {
        private readonly IVerificacionIdentidadService _service;
        public VerificacionIdentidadController(IVerificacionIdentidadService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var dto = await _service.GetByUsuarioIdAsync(usuarioId);

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VerificacionIdentidadDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VerificacionIdentidadDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> Aprobar(int id)
        {
            return await _service.AprobarAsync(id)
                ? NoContent()
                : NotFound();
        }

        [HttpPut("rechazar/{id}")]
        public async Task<IActionResult> Rechazar(int id)
        {
            return await _service.RechazarAsync(id)
                ? NoContent()
                : NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();

        [HttpGet("pendientes")]
        public async Task<IActionResult> Pendientes()
        {
            return Ok(await _service.GetPendientesAsync());
        }
    }
}
