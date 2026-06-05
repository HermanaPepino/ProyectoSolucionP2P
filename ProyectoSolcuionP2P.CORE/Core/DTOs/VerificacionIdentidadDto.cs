using System;

namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class VerificacionIdentidadDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string DocumentoIdentidad { get; set; } = null!;
        public string TipoDocumento { get; set; } = null!;
        public string EstadoVerificacion { get; set; } = null!;
        public DateTime? FechaRegistro { get; set; }
    }
}
