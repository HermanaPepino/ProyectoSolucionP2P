namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class TemporizadorOperacionDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = null!;
    }
}
