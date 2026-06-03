using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Operacion
{
    public int Id { get; set; }

    public int OfertaId { get; set; }

    public int CompradorId { get; set; }

    public int VendedorId { get; set; }

    public decimal Monto { get; set; }

    public string Estado { get; set; } = null!;

    public string CodigoOperacion { get; set; } = null!;

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public DateTime? FechaLiberacion { get; set; }

    public virtual ICollection<Calificacion> Calificacion { get; set; } = new List<Calificacion>();

    public virtual Usuario Comprador { get; set; } = null!;

    public virtual ICollection<ComprobantePago> ComprobantePago { get; set; } = new List<ComprobantePago>();

    public virtual ICollection<Disputa> Disputa { get; set; } = new List<Disputa>();

    public virtual ICollection<Mensaje> Mensaje { get; set; } = new List<Mensaje>();

    public virtual Oferta Oferta { get; set; } = null!;

    public virtual ICollection<TemporizadorOperacion> TemporizadorOperacion { get; set; } = new List<TemporizadorOperacion>();

    public virtual Usuario Vendedor { get; set; } = null!;
}
