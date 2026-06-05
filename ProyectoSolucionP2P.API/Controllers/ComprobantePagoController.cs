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
    public class ComprobantePagoController : ControllerBase
    {
        private readonly IComprobantePagoService _service;

        public ComprobantePagoController(IComprobantePagoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(c => new ComprobantePagoDto
            {
                Id = c.Id,
                OperacionId = c.OperacionId,
                RutaArchivo = c.RutaArchivo,
                FechaSubida = c.FechaSubida
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new ComprobantePagoDto
            {
                Id = ent.Id,
                OperacionId = ent.OperacionId,
                RutaArchivo = ent.RutaArchivo,
                FechaSubida = ent.FechaSubida
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComprobantePagoDto model)
        {
            var entity = new ComprobantePago
            {
                OperacionId = model.OperacionId,
                RutaArchivo = model.RutaArchivo,
                FechaSubida = model.FechaSubida
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new ComprobantePagoDto
            {
                Id = created.Id,
                OperacionId = created.OperacionId,
                RutaArchivo = created.RutaArchivo,
                FechaSubida = created.FechaSubida
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComprobantePagoDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new ComprobantePago
            {
                Id = model.Id,
                OperacionId = model.OperacionId,
                RutaArchivo = model.RutaArchivo,
                FechaSubida = model.FechaSubida
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
