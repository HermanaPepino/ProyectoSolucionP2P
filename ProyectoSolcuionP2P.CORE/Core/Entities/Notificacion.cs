using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Notificacion
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string? Titulo { get; set; }

    public string Mensaje { get; set; } = null!;

    public bool Leida { get; set; }

    public int? OperacionId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Operacion? Operacion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
