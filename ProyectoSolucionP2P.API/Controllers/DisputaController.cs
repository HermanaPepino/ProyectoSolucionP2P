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
    public class DisputaController : ControllerBase
    {
        private readonly IDisputaService _service;
        public DisputaController(IDisputaService service) { _service = service; }

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
        public async Task<IActionResult> Create(DisputaDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        public class AbrirDisputaRequest
        {
            public int OperacionId { get; set; }
            public string Motivo { get; set; } = null!;
        }

        // HU-014: abre la disputa y congela la operación
        [HttpPost("abrir")]
        public async Task<IActionResult> Abrir(AbrirDisputaRequest req)
        {
            var (disputa, error) = await _service.AbrirAsync(req.OperacionId, req.Motivo, UsuarioActualId);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = disputa!.Id }, disputa);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, DisputaDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}