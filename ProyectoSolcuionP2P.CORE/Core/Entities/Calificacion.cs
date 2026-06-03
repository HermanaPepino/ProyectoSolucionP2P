using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Calificacion
{
    public int Id { get; set; }

    public int OperacionId { get; set; }

    public int UsuarioCalificadoId { get; set; }

    public int Puntaje { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Operacion Operacion { get; set; } = null!;

    public virtual Usuario UsuarioCalificado { get; set; } = null!;
}
