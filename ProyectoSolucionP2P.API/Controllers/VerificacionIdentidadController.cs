using Microsoft.AspNetCore.Mvc;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Core.DTOs;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificacionIdentidadController : ControllerBase
    {
        private readonly IVerificacionIdentidadService _service;

        public VerificacionIdentidadController(IVerificacionIdentidadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(v => new VerificacionIdentidadDto
            {
                Id = v.Id,
                UsuarioId = v.UsuarioId,
                DocumentoIdentidad = v.DocumentoIdentidad,
                TipoDocumento = v.TipoDocumento,
                EstadoVerificacion = v.EstadoVerificacion,
                FechaRegistro = v.FechaRegistro
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new VerificacionIdentidadDto
            {
                Id = ent.Id,
                UsuarioId = ent.UsuarioId,
                DocumentoIdentidad = ent.DocumentoIdentidad,
                TipoDocumento = ent.TipoDocumento,
                EstadoVerificacion = ent.EstadoVerificacion,
                FechaRegistro = ent.FechaRegistro
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VerificacionIdentidadDto model)
        {
            var entity = new VerificacionIdentidad
            {
                UsuarioId = model.UsuarioId,
                DocumentoIdentidad = model.DocumentoIdentidad,
                TipoDocumento = model.TipoDocumento,
                EstadoVerificacion = model.EstadoVerificacion,
                FechaRegistro = model.FechaRegistro
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new VerificacionIdentidadDto
            {
                Id = created.Id,
                UsuarioId = created.UsuarioId,
                DocumentoIdentidad = created.DocumentoIdentidad,
                TipoDocumento = created.TipoDocumento,
                EstadoVerificacion = created.EstadoVerificacion,
                FechaRegistro = created.FechaRegistro
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VerificacionIdentidadDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new VerificacionIdentidad
            {
                Id = model.Id,
                UsuarioId = model.UsuarioId,
                DocumentoIdentidad = model.DocumentoIdentidad,
                TipoDocumento = model.TipoDocumento,
                EstadoVerificacion = model.EstadoVerificacion,
                FechaRegistro = model.FechaRegistro
            };

            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
