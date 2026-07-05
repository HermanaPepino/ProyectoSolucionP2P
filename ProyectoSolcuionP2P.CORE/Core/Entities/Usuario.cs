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

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Mensaje> MensajeDestinatarios { get; set; } = new List<Mensaje>();

    public virtual ICollection<Mensaje> MensajeRemitentes { get; set; } = new List<Mensaje>();

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Oferta> Oferta { get; set; } = new List<Oferta>();

    public virtual ICollection<Operacion> OperacionCompradors { get; set; } = new List<Operacion>();

    public virtual ICollection<Operacion> OperacionVendedors { get; set; } = new List<Operacion>();

    public virtual ICollection<ReporteAdministrativo> ReporteAdministrativos { get; set; } = new List<ReporteAdministrativo>();

    public virtual ICollection<VerificacionIdentidad> VerificacionIdentidads { get; set; } = new List<VerificacionIdentidad>();
    public virtual ICollection<UsuarioMetodoPago> UsuarioMetodoPagos { get; set; } = new List<UsuarioMetodoPago>();
}
