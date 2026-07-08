using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Infrastructure.Data;

public partial class CambioSeguroP2pdbContext : DbContext
{
    public CambioSeguroP2pdbContext()
    {
    }

    public CambioSeguroP2pdbContext(DbContextOptions<CambioSeguroP2pdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<ComprobantePago> ComprobantePagos { get; set; }

    public virtual DbSet<Disputa> Disputa { get; set; }

    public virtual DbSet<EvidenciaDisputa> EvidenciaDisputa { get; set; }

    public virtual DbSet<Mensaje> Mensajes { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Moneda> Moneda { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<OfertaMetodoPago> OfertaMetodoPagos { get; set; }

    public virtual DbSet<Oferta> Oferta { get; set; }

    public virtual DbSet<Operacion> Operacions { get; set; }

    public virtual DbSet<ReporteAdministrativo> ReporteAdministrativos { get; set; }

    public virtual DbSet<TemporizadorOperacion> TemporizadorOperacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioMetodoPago> UsuarioMetodoPagos { get; set; }

    public virtual DbSet<VerificacionIdentidad> VerificacionIdentidads { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.ToTable("Calificacion");

            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Operacion).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Operacion");

            entity.HasOne(d => d.UsuarioCalificado).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.UsuarioCalificadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Usuario");
        });

        modelBuilder.Entity<ComprobantePago>(entity =>
        {
            entity.ToTable("ComprobantePago");

            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.ComprobantePagos)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobantePago_Operacion");
        });

        modelBuilder.Entity<Disputa>(entity =>
        {
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Resolucion)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.Disputa)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disputa_Operacion");
        });

        modelBuilder.Entity<EvidenciaDisputa>(entity =>
        {
            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Disputa).WithMany(p => p.EvidenciaDisputa)
                .HasForeignKey(d => d.DisputaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvidenciaDisputa_Disputa");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.ToTable("Mensaje");

            entity.HasIndex(e => e.OperacionId, "IX_Mensaje_Operacion");

            entity.Property(e => e.Contenido)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.MensajeDestinatarios)
                .HasForeignKey(d => d.DestinatarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Destinatario");

            entity.HasOne(d => d.Operacion).WithMany(p => p.Mensajes)
                .HasForeignKey(d => d.OperacionId)
                .HasConstraintName("FK_Mensaje_Operacion");

            entity.HasOne(d => d.Remitente).WithMany(p => p.MensajeRemitentes)
                .HasForeignKey(d => d.RemitenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Remitente");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.ToTable("MetodoPago");

            entity.HasIndex(e => e.Nombre, "UQ_MetodoPago_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Moneda>(entity =>
        {
            entity.HasIndex(e => e.Codigo, "UQ_Moneda_Codigo").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("Notificacion");

            entity.HasIndex(e => new { e.UsuarioId, e.Leida }, "IX_Notificacion_Usuario");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.OperacionId)
                .HasConstraintName("FK_Notificacion_Operacion");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacion_Usuario");
        });

        modelBuilder.Entity<OfertaMetodoPago>(entity =>
        {
            entity.ToTable("OfertaMetodoPago");

            entity.HasIndex(e => new { e.OfertaId, e.MetodoPagoId }, "UQ_OfertaMetodoPago").IsUnique();

            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.DatosRecepcion)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.Instrucciones)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.ResumenPublico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.OfertaMetodoPagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaMetodoPago_MetodoPago");

            entity.HasOne(d => d.Oferta).WithMany(p => p.OfertaMetodoPagos)
                .HasForeignKey(d => d.OfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaMetodoPago_Oferta");
        });

        modelBuilder.Entity<Oferta>(entity =>
        {
            entity.HasIndex(e => e.Estado, "IX_Oferta_Estado");

            entity.HasIndex(e => new { e.MonedaOrigenId, e.MonedaDestinoId }, "IX_Oferta_Monedas");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoDisponible).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoMaximo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoMinimo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TasaCambio).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MonedaDestino).WithMany(p => p.OfertumMonedaDestinos)
                .HasForeignKey(d => d.MonedaDestinoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_MonedaDestino");

            entity.HasOne(d => d.MonedaOrigen).WithMany(p => p.OfertumMonedaOrigens)
                .HasForeignKey(d => d.MonedaOrigenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_MonedaOrigen");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Oferta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_Usuario");
        });

        modelBuilder.Entity<Operacion>(entity =>
        {
            entity.ToTable("Operacion");

            entity.HasIndex(e => e.Estado, "IX_Operacion_Estado");

            entity.HasIndex(e => e.CodigoOperacion, "UQ_Operacion_Codigo").IsUnique();

            entity.Property(e => e.CodigoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaLiberacion).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.DatosPagoComprador)
                .HasMaxLength(700)
                .IsUnicode(false);

            entity.Property(e => e.ResumenPagoComprador)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Comprador).WithMany(p => p.OperacionCompradors)
                .HasForeignKey(d => d.CompradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Comprador");

            entity.HasOne(d => d.Oferta).WithMany(p => p.Operacions)
                .HasForeignKey(d => d.OfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Oferta");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.OperacionVendedors)
                .HasForeignKey(d => d.VendedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Vendedor");

            entity.HasOne(d => d.OfertaMetodoPago).WithMany(p => p.Operacions)
                .HasForeignKey(d => d.OfertaMetodoPagoId)
                .HasConstraintName("FK_Operacion_OfertaMetodoPago");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Operacions)
                .HasForeignKey(d => d.MetodoPagoId)
                .HasConstraintName("FK_Operacion_MetodoPago");

            entity.HasOne(d => d.UsuarioMetodoPago).WithMany(p => p.Operacions)
                .HasForeignKey(d => d.UsuarioMetodoPagoId)
                .HasConstraintName("FK_Operacion_UsuarioMetodoPago");
        });

        modelBuilder.Entity<ReporteAdministrativo>(entity =>
        {
            entity.ToTable("ReporteAdministrativo");

            entity.HasIndex(e => e.GeneradoPorUsuarioId, "IX_ReporteAdministrativo_Usuario");

            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.GeneradoPorUsuario).WithMany(p => p.ReporteAdministrativos)
                .HasForeignKey(d => d.GeneradoPorUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReporteAdministrativo_Usuario");
        });

        modelBuilder.Entity<TemporizadorOperacion>(entity =>
        {
            entity.ToTable("TemporizadorOperacion");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");

            entity.HasOne(d => d.Operacion).WithMany(p => p.TemporizadorOperacions)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemporizadorOperacion_Operacion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ_Usuario_Correo").IsUnique();

            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EstadoVerificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reputacion)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsuarioMetodoPago>(entity =>
        {
            entity.ToTable("UsuarioMetodoPago");

            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.DatosPago)
                .HasMaxLength(700)
                .IsUnicode(false);

            entity.Property(e => e.ResumenPublico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Activo)
                .HasDefaultValue(true);

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioMetodoPagos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioMetodoPago_Usuario");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.UsuarioMetodoPagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioMetodoPago_MetodoPago");
        });

        modelBuilder.Entity<VerificacionIdentidad>(entity =>
        {
            entity.ToTable("VerificacionIdentidad");

            entity.Property(e => e.DocumentoIdentidad)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EstadoVerificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.VerificacionIdentidads)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VerificacionIdentidad_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
