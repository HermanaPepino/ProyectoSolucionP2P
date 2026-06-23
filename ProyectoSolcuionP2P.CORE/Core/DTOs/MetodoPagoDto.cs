namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class MetodoPagoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
