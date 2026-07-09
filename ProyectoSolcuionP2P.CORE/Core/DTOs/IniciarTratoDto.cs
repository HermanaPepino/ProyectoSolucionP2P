namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class IniciarTratoDto
    {
        public int OfertaId { get; set; }

        public decimal Monto { get; set; }

        // Método concreto publicado por el vendedor dentro de la oferta.
        public int? OfertaMetodoPagoId { get; set; }

        // Respaldo para ofertas antiguas: permite buscar por tipo de método.
        public int? MetodoPagoId { get; set; }
    }
}
