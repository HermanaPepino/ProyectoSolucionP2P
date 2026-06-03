using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class TemporizadorOperacion
{
    public int Id { get; set; }

    public int OperacionId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Operacion Operacion { get; set; } = null!;
}
