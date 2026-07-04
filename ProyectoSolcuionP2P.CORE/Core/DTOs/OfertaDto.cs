namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class OfertaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? NombreVendedor { get; set; }
        public int MonedaOrigenId { get; set; }
        public int MonedaDestinoId { get; set; }
        public string MonedaOrigenNombre { get; set; } = null!;
        public string MonedaDestinoNombre { get; set; } = null!;
        public string TipoOperacion { get; set; } = null!;
        public decimal TasaCambio { get; set; }
        public decimal MontoMinimo { get; set; }
        public decimal MontoMaximo { get; set; }
        public decimal? MontoDisponible { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime? FechaCreacion { get; set; }
    }
}
