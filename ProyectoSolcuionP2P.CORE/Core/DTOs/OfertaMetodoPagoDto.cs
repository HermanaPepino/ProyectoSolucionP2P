namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class OfertaMetodoPagoDto
    {
        public int Id { get; set; }

        public int OfertaId { get; set; }

        public int MetodoPagoId { get; set; }

        public string MetodoPagoNombre { get; set; } = string.Empty;

        public string? Alias { get; set; }

        public string? DatosRecepcion { get; set; }

        public string? Instrucciones { get; set; }

        public string? ResumenPublico { get; set; }
    }
}