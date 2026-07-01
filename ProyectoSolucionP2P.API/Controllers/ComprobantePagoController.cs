using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComprobantePagoController : ControllerBase
    {
        private readonly IComprobantePagoService _service;
        public ComprobantePagoController(IComprobantePagoService service) { _service = service; }

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

        [HttpGet("operacion/{operacionId}")]
        public async Task<IActionResult> GetByOperacion(int operacionId)
        {
            var dto = await _service.GetByOperacionIdAsync(operacionId);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComprobantePagoDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        // HU-010: subida real del archivo (JPG, PNG o PDF, máx. 5MB)
        [HttpPost("subir")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> Subir([FromForm] int operacionId, [FromForm] IFormFile archivo)
        {
            var (comprobante, error) = await _service.SubirAsync(operacionId, UsuarioActualId, archivo);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = comprobante!.Id }, comprobante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComprobantePagoDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}