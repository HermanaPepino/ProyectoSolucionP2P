namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    // Lo que se DEVUELVE al cliente (NUNCA incluye Password)
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = null!;
        public string EstadoVerificacion { get; set; } = null!;
        public decimal? Reputacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    // Lo que se RECIBE al registrar (sí incluye Password)
    public class UsuarioRegistroDto
    {
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Telefono { get; set; }
    }

    // Lo que se RECIBE al iniciar sesión
    public class LoginDto
    {
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}