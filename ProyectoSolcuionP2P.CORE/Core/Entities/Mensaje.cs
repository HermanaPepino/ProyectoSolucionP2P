using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Mensaje
{
    public int Id { get; set; }

    public int RemitenteId { get; set; }

    public int DestinatarioId { get; set; }

    public int? OperacionId { get; set; }

    public string Contenido { get; set; } = null!;

    public DateTime? FechaEnvio { get; set; }

    public virtual Usuario Destinatario { get; set; } = null!;

    public virtual Operacion? Operacion { get; set; }

    public virtual Usuario Remitente { get; set; } = null!;
}
