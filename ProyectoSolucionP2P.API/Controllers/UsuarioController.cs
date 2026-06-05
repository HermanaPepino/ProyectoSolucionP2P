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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var dtos = list.Select(u => new UsuarioDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                Password = u.Password,
                Telefono = u.Telefono,
                Rol = u.Rol,
                EstadoVerificacion = u.EstadoVerificacion,
                Reputacion = u.Reputacion,
                FechaRegistro = u.FechaRegistro
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = new UsuarioDto
            {
                Id = ent.Id,
                NombreCompleto = ent.NombreCompleto,
                Correo = ent.Correo,
                Password = ent.Password,
                Telefono = ent.Telefono,
                Rol = ent.Rol,
                EstadoVerificacion = ent.EstadoVerificacion,
                Reputacion = ent.Reputacion,
                FechaRegistro = ent.FechaRegistro
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDto model)
        {
            var entity = new Usuario
            {
                NombreCompleto = model.NombreCompleto,
                Correo = model.Correo,
                Password = model.Password,
                Telefono = model.Telefono,
                Rol = model.Rol,
                EstadoVerificacion = model.EstadoVerificacion,
                Reputacion = model.Reputacion,
                FechaRegistro = model.FechaRegistro
            };

            var created = await _service.CreateAsync(entity);
            var createdDto = new UsuarioDto
            {
                Id = created.Id,
                NombreCompleto = created.NombreCompleto,
                Correo = created.Correo,
                Password = created.Password,
                Telefono = created.Telefono,
                Rol = created.Rol,
                EstadoVerificacion = created.EstadoVerificacion,
                Reputacion = created.Reputacion,
                FechaRegistro = created.FechaRegistro
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioDto model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var entity = new Usuario
            {
                Id = model.Id,
                NombreCompleto = model.NombreCompleto,
                Correo = model.Correo,
                Password = model.Password,
                Telefono = model.Telefono,
                Rol = model.Rol,
                EstadoVerificacion = model.EstadoVerificacion,
                Reputacion = model.Reputacion,
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
