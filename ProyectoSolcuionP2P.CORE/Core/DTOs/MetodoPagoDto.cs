namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class MetodoPagoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public bool Activo { get; set; }
    }

    public class UsuarioMetodoPagoDto
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int MetodoPagoId { get; set; }

        public string MetodoPagoNombre { get; set; } = string.Empty;

        public string Alias { get; set; } = string.Empty;

        // Datos completos del método guardado del propio usuario.
        // Se usan al publicar ofertas para copiar los datos de recepción del vendedor.
        public Dictionary<string, string> DatosPago { get; set; } = new();

        public string ResumenPublico { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }

    public class UsuarioMetodoPagoCreateDto
    {
        public int UsuarioId { get; set; }

        public int MetodoPagoId { get; set; }

        public string Alias { get; set; } = string.Empty;

        public Dictionary<string, string> DatosPago { get; set; } = new();

        public string? ResumenPublico { get; set; }
    }
}
