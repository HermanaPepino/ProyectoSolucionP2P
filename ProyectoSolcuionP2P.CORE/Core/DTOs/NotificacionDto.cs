namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class NotificacionDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Titulo { get; set; }
        public string Mensaje { get; set; } = null!;
        public bool Leida { get; set; }
        public int? OperacionId { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
