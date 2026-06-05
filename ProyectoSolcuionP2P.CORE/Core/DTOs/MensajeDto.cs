using System;

namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class MensajeDto
    {
        public int Id { get; set; }
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        public int? OperacionId { get; set; }
        public string Contenido { get; set; } = null!;
        public DateTime? FechaEnvio { get; set; }
    }
}
