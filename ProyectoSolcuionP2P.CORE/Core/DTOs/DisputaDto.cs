namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class DisputaDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public string Motivo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string? Resolucion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
