namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class DashboardAdministrativoDto
    {
        public int TotalUsuarios { get; set; }

        public int UsuariosActivos { get; set; }

        public int TotalOperaciones { get; set; }

        public int OperacionesCompletadas { get; set; }

        public int TotalDisputas { get; set; }

        public int VerificacionesPendientes { get; set; }

        public int OfertasActivas { get; set; }

        public decimal VolumenIntercambio { get; set; }

        public string MonedaMasUsada { get; set; } = "—";

        public List<MonedaUsoDto> MonedasMasUsadas { get; set; } = new();
    }

    public class MonedaUsoDto
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int CantidadOperaciones { get; set; }

        public decimal Volumen { get; set; }
    }
}
