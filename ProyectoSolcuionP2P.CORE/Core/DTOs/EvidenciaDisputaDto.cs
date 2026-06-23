namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class EvidenciaDisputaDto
    {
        public int Id { get; set; }
        public int DisputaId { get; set; }
        public string RutaArchivo { get; set; } = null!;
        public DateTime? FechaSubida { get; set; }
    }
}
