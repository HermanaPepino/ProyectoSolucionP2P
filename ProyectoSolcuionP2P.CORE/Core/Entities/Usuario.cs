using System;
using System.Collections.Generic;

namespace ProyectoSolucionP2P.CORE.Core.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Rol { get; set; } = null!;

    public string EstadoVerificacion { get; set; } = null!;

    public decimal? Reputacion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Calificacion> Calificacion { get; set; } = new List<Calificacion>();

    public virtual ICollection<Mensaje> MensajeDestinatario { get; set; } = new List<Mensaje>();

    public virtual ICollection<Mensaje> MensajeRemitente { get; set; } = new List<Mensaje>();

    public virtual ICollection<Oferta> Oferta { get; set; } = new List<Oferta>();

    public virtual ICollection<Operacion> OperacionComprador { get; set; } = new List<Operacion>();

    public virtual ICollection<Operacion> OperacionVendedor { get; set; } = new List<Operacion>();

    public virtual ICollection<VerificacionIdentidad> VerificacionIdentidad { get; set; } = new List<VerificacionIdentidad>();
}
