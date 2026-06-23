using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class ReporteAdministrativo
{
    public int Id { get; set; }

    public int GeneradoPorUsuarioId { get; set; }

    public DateTime? FechaGeneracion { get; set; }

    public int TotalOperaciones { get; set; }

    public int TotalDisputas { get; set; }

    public int TotalUsuarios { get; set; }

    public virtual Usuario GeneradoPorUsuario { get; set; } = null!;
}
