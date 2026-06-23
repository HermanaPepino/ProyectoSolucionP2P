using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Moneda
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Oferta> OfertumMonedaDestinos { get; set; } = new List<Oferta>();

    public virtual ICollection<Oferta> OfertumMonedaOrigens { get; set; } = new List<Oferta>();
}
