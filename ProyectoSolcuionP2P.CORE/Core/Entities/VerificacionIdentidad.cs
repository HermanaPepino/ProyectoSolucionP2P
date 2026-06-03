using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class VerificacionIdentidad
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string DocumentoIdentidad { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string EstadoVerificacion { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
