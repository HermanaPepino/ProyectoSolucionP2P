namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class CalificacionDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public int UsuarioCalificadoId { get; set; }
        public int Puntaje { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
