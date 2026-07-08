namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class UsuarioReputacionDto
    {
        public int UsuarioId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal CalificacionPromedio { get; set; }

        public int CantidadCalificaciones { get; set; }

        public int OperacionesCompletadas { get; set; }

        public decimal TasaExito { get; set; }

        public List<string> Comentarios { get; set; } = new();
    }
}