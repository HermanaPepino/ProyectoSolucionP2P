namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class DisputaHistorialDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public string CodigoOperacion { get; set; } = string.Empty;
        public string EstadoOperacion { get; set; } = string.Empty;
        public decimal Monto { get; set; }

        public int CompradorId { get; set; }
        public int VendedorId { get; set; }
        public string CompradorNombre { get; set; } = string.Empty;
        public string VendedorNombre { get; set; } = string.Empty;
        public string MetodoPagoNombre { get; set; } = string.Empty;

        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Resolucion { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaLiberacion { get; set; }

        public List<EvidenciaDisputaDto> Evidencias { get; set; } = new();
    }
}
