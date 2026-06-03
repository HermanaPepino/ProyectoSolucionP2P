using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Oferta
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int MonedaOrigenId { get; set; }

    public int MonedaDestinoId { get; set; }

    public string TipoOperacion { get; set; } = null!;

    public decimal TasaCambio { get; set; }

    public decimal MontoMinimo { get; set; }

    public decimal MontoMaximo { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public virtual Moneda MonedaDestino { get; set; } = null!;

    public virtual Moneda MonedaOrigen { get; set; } = null!;

    public virtual ICollection<Operacion> Operacion { get; set; } = new List<Operacion>();

    public virtual Usuario Usuario { get; set; } = null!;
}
