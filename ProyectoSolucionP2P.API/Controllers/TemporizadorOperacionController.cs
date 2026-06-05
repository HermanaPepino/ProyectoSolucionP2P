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
    public class TemporizadorOperacionController : ControllerBase
    {
        private readonly ITemporizadorOperacionService _service;

        public TemporizadorOperacionController(ITemporizadorOperacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(t => new TemporizadorOperacionDto
            {
                Id = t.Id,
                OperacionId = t.OperacionId,
                FechaInicio = t.FechaInicio,
                FechaFin = t.FechaFin,
                Estado = t.Estado
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new TemporizadorOperacionDto
            {
                Id = ent.Id,
                OperacionId = ent.OperacionId,
                FechaInicio = ent.FechaInicio,
                FechaFin = ent.FechaFin,
                Estado = ent.Estado
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemporizadorOperacionDto model)
        {
            var entity = new TemporizadorOperacion
            {
                OperacionId = model.OperacionId,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                Estado = model.Estado
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new TemporizadorOperacionDto
            {
                Id = created.Id,
                OperacionId = created.OperacionId,
                FechaInicio = created.FechaInicio,
                FechaFin = created.FechaFin,
                Estado = created.Estado
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TemporizadorOperacionDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new TemporizadorOperacion
            {
                Id = model.Id,
                OperacionId = model.OperacionId,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                Estado = model.Estado
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
