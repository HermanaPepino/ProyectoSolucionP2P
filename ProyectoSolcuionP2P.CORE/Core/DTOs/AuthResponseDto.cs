namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UsuarioDto Usuario { get; set; } = null!;
    }
}