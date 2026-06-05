using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Entities;

namespace ProyectoSolucionP2P.CORE.Infraestructure.Data;

public partial class CambioSeguroP2pdbContext : DbContext
{
    public CambioSeguroP2pdbContext()
    {
    }

    public CambioSeguroP2pdbContext(DbContextOptions<CambioSeguroP2pdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacion { get; set; }

    public virtual DbSet<ComprobantePago> ComprobantePago { get; set; }

    public virtual DbSet<Disputa> Disputa { get; set; }

    public virtual DbSet<Mensaje> Mensaje { get; set; }

    public virtual DbSet<Moneda> Moneda { get; set; }

    public virtual DbSet<Oferta> Oferta { get; set; }

    public virtual DbSet<Operacion> Operacion { get; set; }

    public virtual DbSet<ReporteAdministrativo> ReporteAdministrativo { get; set; }

    public virtual DbSet<TemporizadorOperacion> TemporizadorOperacion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<VerificacionIdentidad> VerificacionIdentidad { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Califica__3214EC0729320536");

            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Operacion).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Operacion");

            entity.HasOne(d => d.UsuarioCalificado).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.UsuarioCalificadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Usuario");
        });

        modelBuilder.Entity<ComprobantePago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comproba__3214EC07B26502B8");

            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.ComprobantePago)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobantePago_Operacion");
        });

        modelBuilder.Entity<Disputa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Disputa__3214EC07D767B6A3");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Resolucion)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.Disputa)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disputa_Operacion");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mensaje__3214EC07CC598DD2");

            entity.Property(e => e.Contenido)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.MensajeDestinatario)
                .HasForeignKey(d => d.DestinatarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Destinatario");

            entity.HasOne(d => d.Operacion).WithMany(p => p.Mensaje)
                .HasForeignKey(d => d.OperacionId)
                .HasConstraintName("FK_Mensaje_Operacion");

            entity.HasOne(d => d.Remitente).WithMany(p => p.MensajeRemitente)
                .HasForeignKey(d => d.RemitenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Remitente");
        });

        modelBuilder.Entity<Moneda>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Moneda__3214EC0789B96330");

            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Oferta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Oferta__3214EC07BB1ED70F");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoMaximo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoMinimo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TasaCambio).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MonedaDestino).WithMany(p => p.OfertaMonedaDestino)
                .HasForeignKey(d => d.MonedaDestinoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_MonedaDestino");

            entity.HasOne(d => d.MonedaOrigen).WithMany(p => p.OfertaMonedaOrigen)
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
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3214EC075CC94748");

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

            entity.HasOne(d => d.Comprador).WithMany(p => p.OperacionComprador)
                .HasForeignKey(d => d.CompradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Comprador");

            entity.HasOne(d => d.Oferta).WithMany(p => p.Operacion)
                .HasForeignKey(d => d.OfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Oferta");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.OperacionVendedor)
                .HasForeignKey(d => d.VendedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_Vendedor");
        });

        modelBuilder.Entity<ReporteAdministrativo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReporteA__3214EC07950FE5BD");

            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TemporizadorOperacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Temporiz__3214EC0746B71932");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");

            entity.HasOne(d => d.Operacion).WithMany(p => p.TemporizadorOperacion)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemporizadorOperacion_Operacion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07A0DDEA0C");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A19888EBA05").IsUnique();

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

        modelBuilder.Entity<VerificacionIdentidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Verifica__3214EC07DE341B0F");

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

            entity.HasOne(d => d.Usuario).WithMany(p => p.VerificacionIdentidad)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VerificacionIdentidad_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
