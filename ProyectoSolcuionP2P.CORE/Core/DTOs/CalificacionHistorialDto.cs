namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class CalificacionHistorialDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public string CodigoOperacion { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Dada o Recibida
        public int Puntaje { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public int UsuarioCalificadoId { get; set; }
        public string UsuarioCalificadoNombre { get; set; } = string.Empty;
        public int UsuarioCalificadorId { get; set; }
        public string UsuarioCalificadorNombre { get; set; } = string.Empty;

        public decimal Monto { get; set; }
        public string EstadoOperacion { get; set; } = string.Empty;
    }
}
