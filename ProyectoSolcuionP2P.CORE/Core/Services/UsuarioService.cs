using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(MapToDto);
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u == null ? null : MapToDto(u);
        }

        // HU-001: Registro
        public async Task<UsuarioDto?> RegistrarAsync(UsuarioRegistroDto dto)
        {
            // correo único
            var existe = await _repo.GetByCorreoAsync(dto.Correo);
            if (existe != null) return null;

            var nuevo = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // se guarda el HASH
                Telefono = dto.Telefono,
                Rol = "Usuario",
                EstadoVerificacion = "Pendiente",
                Reputacion = 0,
                FechaRegistro = DateTime.Now
            };

            var creado = await _repo.CreateAsync(nuevo);
            return MapToDto(creado);
        }

        // HU-002: Login
        public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
        {
            var u = await _repo.GetByCorreoAsync(dto.Correo);
            if (u == null) return null;

            bool ok = BCrypt.Net.BCrypt.Verify(dto.Password, u.Password);
            return ok ? MapToDto(u) : null;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioRegistroDto dto)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return false;

            u.NombreCompleto = dto.NombreCompleto;
            u.Telefono = dto.Telefono;
            await _repo.UpdateAsync(u);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        // Mapea Entidad -> DTO (sin exponer el Password)
        private static UsuarioDto MapToDto(Usuario u) => new UsuarioDto
        {
            Id = u.Id,
            NombreCompleto = u.NombreCompleto,
            Correo = u.Correo,
            Telefono = u.Telefono,
            Rol = u.Rol,
            EstadoVerificacion = u.EstadoVerificacion,
            Reputacion = u.Reputacion,
            FechaRegistro = u.FechaRegistro
        };
    }
}