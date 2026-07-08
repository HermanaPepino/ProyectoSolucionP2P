namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class UsuarioReputacionDto
    {
        public int UsuarioId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal CalificacionPromedio { get; set; }

        public int CantidadCalificaciones { get; set; }

        public int OperacionesCompletadas { get; set; }

        // Operaciones que ya terminaron y sí deben entrar al cálculo de tasa de éxito.
        // No cuenta operaciones en proceso, pago enviado o en disputa porque aún no tienen resultado final.
        public int OperacionesFinalizadas { get; set; }

        public decimal TasaExito { get; set; }

        // Promedio de duración de operaciones completadas, desde FechaInicio hasta FechaFin.
        public decimal TiempoPromedioOperacionMinutos { get; set; }

        public List<string> Comentarios { get; set; } = new();
    }
}
