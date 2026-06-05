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
    public class MensajeController : ControllerBase
    {
        private readonly IMensajeService _service;

        public MensajeController(IMensajeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(m => new MensajeDto
            {
                Id = m.Id,
                RemitenteId = m.RemitenteId,
                DestinatarioId = m.DestinatarioId,
                OperacionId = m.OperacionId,
                Contenido = m.Contenido,
                FechaEnvio = m.FechaEnvio
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new MensajeDto
            {
                Id = ent.Id,
                RemitenteId = ent.RemitenteId,
                DestinatarioId = ent.DestinatarioId,
                OperacionId = ent.OperacionId,
                Contenido = ent.Contenido,
                FechaEnvio = ent.FechaEnvio
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MensajeDto model)
        {
            var entity = new Mensaje
            {
                RemitenteId = model.RemitenteId,
                DestinatarioId = model.DestinatarioId,
                OperacionId = model.OperacionId,
                Contenido = model.Contenido,
                FechaEnvio = model.FechaEnvio
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new MensajeDto
            {
                Id = created.Id,
                RemitenteId = created.RemitenteId,
                DestinatarioId = created.DestinatarioId,
                OperacionId = created.OperacionId,
                Contenido = created.Contenido,
                FechaEnvio = created.FechaEnvio
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MensajeDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new Mensaje
            {
                Id = model.Id,
                RemitenteId = model.RemitenteId,
                DestinatarioId = model.DestinatarioId,
                OperacionId = model.OperacionId,
                Contenido = model.Contenido,
                FechaEnvio = model.FechaEnvio
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
