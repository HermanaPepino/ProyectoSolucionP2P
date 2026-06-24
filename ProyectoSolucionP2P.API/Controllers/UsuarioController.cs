using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var u = await _service.GetByIdAsync(id);
            return u == null ? NotFound() : Ok(u);
        }

        // HU-001
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(UsuarioRegistroDto dto)
        {
            var creado = await _service.RegistrarAsync(dto);
            if (creado == null) return BadRequest("El correo ya está registrado.");
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        // HU-002
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var resp = await _service.LoginAsync(dto);
            return resp == null ? Unauthorized("Correo o contraseña incorrectos.") : Ok(resp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioRegistroDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}