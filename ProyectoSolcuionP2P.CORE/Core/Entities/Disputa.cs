using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Disputa
{
    public int Id { get; set; }

    public int OperacionId { get; set; }

    public string Motivo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Resolucion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<EvidenciaDisputa> EvidenciaDisputa { get; set; } = new List<EvidenciaDisputa>();

    public virtual Operacion Operacion { get; set; } = null!;
}
