using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class ComprobantePago
{
    public int Id { get; set; }

    public int OperacionId { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public DateTime? FechaSubida { get; set; }

    public virtual Operacion Operacion { get; set; } = null!;
}
