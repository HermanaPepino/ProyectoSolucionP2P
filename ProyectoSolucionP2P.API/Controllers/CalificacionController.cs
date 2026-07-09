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
    public class CalificacionController : ControllerBase
    {
        private readonly ICalificacionService _service;

        public CalificacionController(ICalificacionService service)
        {
            _service = service;
        }
        h
        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CalificacionDto dto)
        {
            if (User.IsInRole("Administrador"))
                return BadRequest(new { mensaje = "El administrador no puede registrar calificaciones." });

            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CalificacionDto dto)
        {
            if (User.IsInRole("Administrador"))
                return BadRequest(new { mensaje = "El administrador no puede modificar calificaciones como usuario." });

            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}