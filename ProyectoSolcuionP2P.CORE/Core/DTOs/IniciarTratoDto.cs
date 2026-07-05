namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class IniciarTratoDto
    {
        public int OfertaId { get; set; }

        public decimal Monto { get; set; }

        public int? OfertaMetodoPagoId { get; set; }

        public int? MetodoPagoId { get; set; }

        public int? UsuarioMetodoPagoId { get; set; }

        public bool GuardarMetodoComprador { get; set; }

        public Dictionary<string, string>? DatosPagoComprador { get; set; }
    }
}