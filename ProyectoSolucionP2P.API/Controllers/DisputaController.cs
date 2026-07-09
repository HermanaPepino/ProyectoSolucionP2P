using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private const long MaxArchivoBytes = 5 * 1024 * 1024;

        private static readonly string[] ExtensionesPermitidas =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".pdf"
        };

        private readonly IDisputaService _service;
        private readonly IEvidenciaDisputaService _evidenciaService;
        private readonly IWebHostEnvironment _env;

        public DisputaController(
            IDisputaService service,
            IEvidenciaDisputaService evidenciaService,
            IWebHostEnvironment env)
        {
            _service = service;
            _evidenciaService = evidenciaService;
            _env = env;
        }

        private int UsuarioActualId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("admin/historial")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAllHistorial() => Ok(await _service.GetAllHistorialAsync());

        [HttpGet("mis-disputas")]
        public async Task<IActionResult> GetMisDisputas()
            => Ok(await _service.GetMisDisputasAsync(UsuarioActualId));

        [HttpGet("por-operacion/{operacionId:int}")]
        public async Task<IActionResult> GetByOperacion(int operacionId)
        {
            var dto = await _service.GetByOperacionForUserAsync(operacionId, UsuarioActualId);
            return dto == null ? NotFound(new { mensaje = "Esta operación no tiene disputa registrada." }) : Ok(dto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(DisputaDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        public class AbrirDisputaRequest
        {
            public int OperacionId { get; set; }
            public string Motivo { get; set; } = string.Empty;
            public IFormFile? Archivo { get; set; }
        }

        // HU-014: abre disputa con evidencia obligatoria.
        [HttpPost("abrir")]
        [RequestSizeLimit(MaxArchivoBytes + 1024 * 1024)]
        public async Task<IActionResult> Abrir([FromForm] AbrirDisputaRequest req)
        {
            var errorArchivo = ValidarArchivo(req.Archivo);
            if (errorArchivo != null)
                return BadRequest(new { mensaje = errorArchivo });

            var (disputa, error) = await _service.AbrirAsync(req.OperacionId, req.Motivo, UsuarioActualId);
            if (error != null) return BadRequest(new { mensaje = error });

            var rutaPublica = await GuardarArchivoAsync(disputa!.Id, req.Archivo!);

            var evidencia = await _evidenciaService.CreateAsync(new EvidenciaDisputaDto
            {
                DisputaId = disputa.Id,
                RutaArchivo = rutaPublica,
                FechaSubida = DateTime.Now
            });

            return CreatedAtAction(nameof(GetById), new { id = disputa.Id }, new
            {
                disputa,
                evidencia,
                mensaje = "Disputa registrada con evidencia. Administración revisará el caso."
            });
        }


        public class ResolverDisputaRequest
        {
            public string Estado { get; set; } = string.Empty;
            public string Resolucion { get; set; } = string.Empty;
        }

        [HttpPut("{id:int}/resolver")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Resolver(int id, ResolverDisputaRequest req)
        {
            var (ok, error) = await _service.ResolverAsync(id, req.Estado, req.Resolucion);
            if (!ok) return BadRequest(new { mensaje = error });
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, DisputaDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();

        private static string? ValidarArchivo(IFormFile? archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return "Debes adjuntar una evidencia para abrir la disputa.";

            if (archivo.Length > MaxArchivoBytes)
                return "La evidencia no puede pesar más de 5 MB.";

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!ExtensionesPermitidas.Contains(extension))
                return "Formato inválido. Adjunta JPG, PNG o PDF.";

            return null;
        }

        private async Task<string> GuardarArchivoAsync(int disputaId, IFormFile archivo)
        {
            var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var carpeta = Path.Combine(root, "uploads", "disputas");
            Directory.CreateDirectory(carpeta);

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            var nombreArchivo = $"{disputaId}_{Guid.NewGuid():N}{extension}";
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            await using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return $"/uploads/disputas/{nombreArchivo}";
        }
    }
}
