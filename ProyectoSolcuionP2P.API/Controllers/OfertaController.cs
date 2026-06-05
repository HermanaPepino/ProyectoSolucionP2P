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
    public class OfertaController : ControllerBase
    {
        private readonly IOfertaService _service;

        public OfertaController(IOfertaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(o => new OfertaDto
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                MonedaOrigenId = o.MonedaOrigenId,
                MonedaDestinoId = o.MonedaDestinoId,
                TipoOperacion = o.TipoOperacion,
                TasaCambio = o.TasaCambio,
                MontoMinimo = o.MontoMinimo,
                MontoMaximo = o.MontoMaximo,
                Estado = o.Estado,
                FechaCreacion = o.FechaCreacion
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new OfertaDto
            {
                Id = ent.Id,
                UsuarioId = ent.UsuarioId,
                MonedaOrigenId = ent.MonedaOrigenId,
                MonedaDestinoId = ent.MonedaDestinoId,
                TipoOperacion = ent.TipoOperacion,
                TasaCambio = ent.TasaCambio,
                MontoMinimo = ent.MontoMinimo,
                MontoMaximo = ent.MontoMaximo,
                Estado = ent.Estado,
                FechaCreacion = ent.FechaCreacion
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfertaDto model)
        {
            var entity = new Oferta
            {
                UsuarioId = model.UsuarioId,
                MonedaOrigenId = model.MonedaOrigenId,
                MonedaDestinoId = model.MonedaDestinoId,
                TipoOperacion = model.TipoOperacion,
                TasaCambio = model.TasaCambio,
                MontoMinimo = model.MontoMinimo,
                MontoMaximo = model.MontoMaximo,
                Estado = model.Estado,
                FechaCreacion = model.FechaCreacion
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new OfertaDto
            {
                Id = created.Id,
                UsuarioId = created.UsuarioId,
                MonedaOrigenId = created.MonedaOrigenId,
                MonedaDestinoId = created.MonedaDestinoId,
                TipoOperacion = created.TipoOperacion,
                TasaCambio = created.TasaCambio,
                MontoMinimo = created.MontoMinimo,
                MontoMaximo = created.MontoMaximo,
                Estado = created.Estado,
                FechaCreacion = created.FechaCreacion
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OfertaDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new Oferta
            {
                Id = model.Id,
                UsuarioId = model.UsuarioId,
                MonedaOrigenId = model.MonedaOrigenId,
                MonedaDestinoId = model.MonedaDestinoId,
                TipoOperacion = model.TipoOperacion,
                TasaCambio = model.TasaCambio,
                MontoMinimo = model.MontoMinimo,
                MontoMaximo = model.MontoMaximo,
                Estado = model.Estado,
                FechaCreacion = model.FechaCreacion
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
