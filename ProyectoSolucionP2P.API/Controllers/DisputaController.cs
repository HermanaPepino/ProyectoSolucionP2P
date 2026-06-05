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
    public class DisputaController : ControllerBase
    {
        private readonly IDisputaService _service;

        public DisputaController(IDisputaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(d => new DisputaDto
            {
                Id = d.Id,
                OperacionId = d.OperacionId,
                Motivo = d.Motivo,
                Estado = d.Estado,
                Resolucion = d.Resolucion,
                FechaRegistro = d.FechaRegistro
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new DisputaDto
            {
                Id = ent.Id,
                OperacionId = ent.OperacionId,
                Motivo = ent.Motivo,
                Estado = ent.Estado,
                Resolucion = ent.Resolucion,
                FechaRegistro = ent.FechaRegistro
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DisputaDto model)
        {
            var entity = new Disputa
            {
                OperacionId = model.OperacionId,
                Motivo = model.Motivo,
                Estado = model.Estado,
                Resolucion = model.Resolucion,
                FechaRegistro = model.FechaRegistro
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new DisputaDto
            {
                Id = created.Id,
                OperacionId = created.OperacionId,
                Motivo = created.Motivo,
                Estado = created.Estado,
                Resolucion = created.Resolucion,
                FechaRegistro = created.FechaRegistro
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DisputaDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new Disputa
            {
                Id = model.Id,
                OperacionId = model.OperacionId,
                Motivo = model.Motivo,
                Estado = model.Estado,
                Resolucion = model.Resolucion,
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
