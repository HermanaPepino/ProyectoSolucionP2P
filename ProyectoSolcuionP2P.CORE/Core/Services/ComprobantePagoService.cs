using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class ComprobantePagoService : IComprobantePagoService
    {
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".pdf" };
        private const long TamanoMaximoBytes = 5 * 1024 * 1024; // 5 MB

        private readonly IComprobantePagoRepository _repo;
        private readonly IOperacionRepository _operacionRepo;
        private readonly IOperacionService _operacionService;
        private readonly IWebHostEnvironment _env;

        public ComprobantePagoService(
            IComprobantePagoRepository repo,
            IOperacionRepository operacionRepo,
            IOperacionService operacionService,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _operacionRepo = operacionRepo;
            _operacionService = operacionService;
            _env = env;
        }

        public async Task<IEnumerable<ComprobantePagoDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<ComprobantePagoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<ComprobantePagoDto?> GetByOperacionIdAsync(int operacionId)
        {
            var e = await _repo.GetByOperacionIdAsync(operacionId);
            return e == null ? null : MapToDto(e);
        }

        public async Task<ComprobantePagoDto> CreateAsync(ComprobantePagoDto dto)
        {
            var e = new ComprobantePago
            {
                OperacionId = dto.OperacionId,
                RutaArchivo = dto.RutaArchivo,
                FechaSubida = dto.FechaSubida
            };
            var creado = await _repo.CreateAsync(e);
            dto.Id = creado.Id;
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, ComprobantePagoDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            e.OperacionId = dto.OperacionId;
            e.RutaArchivo = dto.RutaArchivo;
            e.FechaSubida = dto.FechaSubida;
            await _repo.UpdateAsync(e);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        // -----------------------------------------------------------
        // HU-010: subida real del comprobante (imagen o PDF)
        // -----------------------------------------------------------
        public async Task<(ComprobantePagoDto? comprobante, string? error)> SubirAsync(int operacionId, int usuarioId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return (null, "Selecciona un archivo.");

            if (archivo.Length > TamanoMaximoBytes)
                return (null, "El archivo no puede superar los 5 MB.");

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!ExtensionesPermitidas.Contains(extension))
                return (null, "Formato no permitido. Usa JPG, PNG o PDF.");

            var operacion = await _operacionRepo.GetByIdAsync(operacionId);
            if (operacion == null)
                return (null, "La operación no existe.");

            if (operacion.CompradorId != usuarioId)
                return (null, "Solo el comprador puede subir el comprobante de esta operación.");

            if (operacion.Estado != "En proceso")
                return (null, "Esta operación ya no admite subir un comprobante.");

            var carpeta = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "comprobantes");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{operacionId}_{Guid.NewGuid():N}{extension}";
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var rutaPublica = $"/uploads/comprobantes/{nombreArchivo}";

            var entidad = new ComprobantePago
            {
                OperacionId = operacionId,
                RutaArchivo = rutaPublica,
                FechaSubida = DateTime.Now
            };
            var creado = await _repo.CreateAsync(entidad);

            // HU-010/011: marca la operación como "Pago enviado" y notifica al vendedor
            var (ok, error) = await _operacionService.MarcarPagoEnviadoAsync(operacionId, usuarioId);
            if (!ok) return (null, error);

            return (MapToDto(creado), null);
        }

        private static ComprobantePagoDto MapToDto(ComprobantePago e) => new ComprobantePagoDto
        {
            Id = e.Id,
            OperacionId = e.OperacionId,
            RutaArchivo = e.RutaArchivo,
            FechaSubida = e.FechaSubida
        };
    }
}