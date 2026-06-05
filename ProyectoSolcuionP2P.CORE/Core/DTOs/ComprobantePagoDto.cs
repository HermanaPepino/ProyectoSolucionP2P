using System;

namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class ComprobantePagoDto
    {
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public string RutaArchivo { get; set; } = null!;
        public DateTime? FechaSubida { get; set; }
    }
}
