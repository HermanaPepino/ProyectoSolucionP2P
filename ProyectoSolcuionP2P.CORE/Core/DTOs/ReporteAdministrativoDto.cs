namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class ReporteAdministrativoDto
    {
        public int Id { get; set; }
        public int GeneradoPorUsuarioId { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public int TotalOperaciones { get; set; }
        public int TotalDisputas { get; set; }
        public int TotalUsuarios { get; set; }
    }
}
