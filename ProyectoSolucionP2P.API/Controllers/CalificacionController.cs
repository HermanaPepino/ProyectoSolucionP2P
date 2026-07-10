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

        // Obtiene el identificador del usuario autenticado desde el token JWT.
        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Solo administración puede consultar todas las calificaciones del sistema.
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        // Devuelve las calificaciones dadas y recibidas por el usuario autenticado.
        // Por defecto muestra los últimos 60 días.
        [HttpGet("mis-calificaciones")]
        public async Task<IActionResult> GetMisCalificaciones([FromQuery] int dias = 60)
        {
            // Evita valores inválidos o rangos exageradamente grandes.
            var diasValidos = Math.Clamp(dias, 1, 365);

            var historial = await _service.GetMisCalificacionesAsync(
                UsuarioActualId,
                diasValidos
            );

            return Ok(historial);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);

            return dto == null
                ? NotFound(new { mensaje = "La calificación no existe." })
                : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CalificacionDto dto)
        {
            if (User.IsInRole("Administrador"))
            {
                return BadRequest(new
                {
                    mensaje = "El administrador no puede registrar calificaciones."
                });
            }

            try
            {
                // Se utiliza la sobrecarga que valida al usuario autenticado,
                // la operación completada y la contraparte correspondiente.
                var creado = await _service.CreateAsync(dto, UsuarioActualId);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = creado.Id },
                    creado
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CalificacionDto dto)
        {
            if (User.IsInRole("Administrador"))
            {
                return BadRequest(new
                {
                    mensaje = "El administrador no puede modificar calificaciones como usuario."
                });
            }

            try
            {
                return await _service.UpdateAsync(id, dto)
                    ? NoContent()
                    : NotFound(new { mensaje = "La calificación no existe." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id)
                ? NoContent()
                : NotFound(new { mensaje = "La calificación no existe." });
    }
}