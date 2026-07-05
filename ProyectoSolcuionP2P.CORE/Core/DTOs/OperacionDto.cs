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

        public int? OfertaMetodoPagoId { get; set; }

        public int? MetodoPagoId { get; set; }

        public string? MetodoPagoNombre { get; set; }

        public int? UsuarioMetodoPagoId { get; set; }

        public string? DatosPagoComprador { get; set; }

        public string? ResumenPagoComprador { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaLiberacion { get; set; }

        public DateTime? TemporizadorFechaFin { get; set; }

        public string? TemporizadorEstado { get; set; }

        public int? SegundosRestantes { get; set; }
    }
}