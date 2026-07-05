using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class UsuarioMetodoPago
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int MetodoPagoId { get; set; }

    public string Alias { get; set; } = null!;

    public string DatosPago { get; set; } = null!;

    public string ResumenPublico { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual ICollection<Operacion> Operacions { get; set; } = new List<Operacion>();
}