using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class OfertaMetodoPago
{
    public int Id { get; set; }

    public int OfertaId { get; set; }

    public int MetodoPagoId { get; set; }

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Oferta Oferta { get; set; } = null!;
}
