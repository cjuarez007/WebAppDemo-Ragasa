using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class DBC_incrementos : DbContext
{
    public DBC_incrementos()
    {
    }

    public DBC_incrementos(DbContextOptions<DBC_incrementos> options)
        : base(options)
    {
    }

    public virtual DbSet<Incremento> Incrementos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=ESILENOVO05\\SQLEXPRESS02;Database=demo_web;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Incremento>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("incrementos");

            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.IdPuesto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id_puesto");
            entity.Property(e => e.Jefe)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("jefe");
            entity.Property(e => e.Nn).HasColumnName("nn");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PorcOtorgado)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porc_otorgado");
            entity.Property(e => e.PorcOtorgadoJefe)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porc_otorgado_jefe");
            entity.Property(e => e.PorcSugerido)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porc_sugerido");
            entity.Property(e => e.Puesto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("puesto");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Sem12023)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("sem1_2023");
            entity.Property(e => e.Sem22023)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("sem2_2023");
            entity.Property(e => e.SueldoActual)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldo_actual");
            entity.Property(e => e.SueldoNuevo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldo_nuevo");
            entity.Property(e => e.SueldoSugerido)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldo_sugerido");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
