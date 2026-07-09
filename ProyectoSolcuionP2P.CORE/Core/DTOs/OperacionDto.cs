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

        // Datos publicados por el vendedor para recibir el pago manual.
        public string? AliasPagoVendedor { get; set; }

        public string? DatosRecepcionVendedor { get; set; }

        public string? InstruccionesPagoVendedor { get; set; }

        public string? ResumenPagoVendedor { get; set; }

        // Se conservan por compatibilidad con la BD/parches anteriores, pero el flujo P2P manual
        // ya no obliga al comprador a registrar un método propio para iniciar operación.
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
