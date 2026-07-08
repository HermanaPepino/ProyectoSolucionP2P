using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EvidenciaDisputaController : ControllerBase
    {
        private const long MaxArchivoBytes = 5 * 1024 * 1024;

        private static readonly string[] ExtensionesPermitidas =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".pdf"
        };

        private readonly IEvidenciaDisputaService _service;
        private readonly IDisputaService _disputaService;
        private readonly IOperacionService _operacionService;
        private readonly IWebHostEnvironment _env;

        public EvidenciaDisputaController(
            IEvidenciaDisputaService service,
            IDisputaService disputaService,
            IOperacionService operacionService,
            IWebHostEnvironment env)
        {
            _service = service;
            _disputaService = disputaService;
            _operacionService = operacionService;
            _env = env;
        }

        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(EvidenciaDisputaDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPost("subir")]
        public async Task<IActionResult> Subir([FromForm] int disputaId, [FromForm] IFormFile archivo)
        {
            if (disputaId <= 0)
                return BadRequest(new { mensaje = "La disputa es obligatoria." });

            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "Selecciona un archivo de evidencia." });

            if (archivo.Length > MaxArchivoBytes)
                return BadRequest(new { mensaje = "La evidencia no puede pesar más de 5 MB." });

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!ExtensionesPermitidas.Contains(extension))
                return BadRequest(new { mensaje = "Formato inválido. Usa JPG, PNG o PDF." });

            var disputa = await _disputaService.GetByIdAsync(disputaId);
            if (disputa == null)
                return NotFound(new { mensaje = "La disputa no existe." });

            var operacion = await _operacionService.GetByIdAsync(disputa.OperacionId);
            if (operacion == null)
                return NotFound(new { mensaje = "La operación asociada a la disputa no existe." });

            var participa = operacion.CompradorId == UsuarioActualId || operacion.VendedorId == UsuarioActualId;
            if (!participa && !User.IsInRole("Administrador"))
                return Forbid();

            var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var carpeta = Path.Combine(root, "uploads", "disputas");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{disputaId}_{Guid.NewGuid():N}{extension}";
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            await using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var rutaPublica = $"/uploads/disputas/{nombreArchivo}";

            var creado = await _service.CreateAsync(new EvidenciaDisputaDto
            {
                DisputaId = disputaId,
                RutaArchivo = rutaPublica,
                FechaSubida = DateTime.Now
            });

            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, EvidenciaDisputaDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
