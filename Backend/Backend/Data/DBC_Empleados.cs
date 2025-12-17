using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class DBC_Empleados : DbContext
{
    public DBC_Empleados()
    {
    }

    public DBC_Empleados(DbContextOptions<DBC_Empleados> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleado> Empleados { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=ESILENOVO05\\SQLEXPRESS02;Database=demo_web;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Antiguedad).HasColumnName("antiguedad");
            entity.Property(e => e.Cia).HasColumnName("cia");
            entity.Property(e => e.Departamento)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("departamento");
            entity.Property(e => e.Fechaingreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaingreso");            
            entity.Property(e => e.Jefe).HasColumnName("jefe");
            entity.Property(e => e.Nivel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nivel");
            entity.Property(e => e.Nivelnum).HasColumnName("nivelnum");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Nomina).HasColumnName("nomina");
            entity.Property(e => e.Puesto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("puesto");
            entity.Property(e => e.Segmento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("segmento");
            entity.Property(e => e.Tipotrab)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipotrab");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
