using System;

namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = null!;
        public string EstadoVerificacion { get; set; } = null!;
        public decimal? Reputacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
