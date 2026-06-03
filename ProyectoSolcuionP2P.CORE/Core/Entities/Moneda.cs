using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Moneda
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Oferta> OfertaMonedaDestino { get; set; } = new List<Oferta>();

    public virtual ICollection<Oferta> OfertaMonedaOrigen { get; set; } = new List<Oferta>();
}
