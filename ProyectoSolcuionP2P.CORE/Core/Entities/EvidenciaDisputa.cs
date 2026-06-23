using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class EvidenciaDisputa
{
    public int Id { get; set; }

    public int DisputaId { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public DateTime? FechaSubida { get; set; }

    public virtual Disputa Disputa { get; set; } = null!;
}
