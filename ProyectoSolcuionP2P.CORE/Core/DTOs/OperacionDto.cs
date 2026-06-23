namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class OperacionDto
    {
        public int Id { get; set; }
        public int OfertaId { get; set; }
        public int CompradorId { get; set; }
        public int VendedorId { get; set; }
        public string CompradorNombre { get; set; } = null!;
        public string VendedorNombre { get; set; } = null!;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = null!;
        public string CodigoOperacion { get; set; } = null!;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaLiberacion { get; set; }
    }
}
