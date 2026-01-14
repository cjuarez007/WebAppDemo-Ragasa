using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class DBC_resultados : DbContext
{
    public DBC_resultados()
    {
    }

    public DBC_resultados(DbContextOptions<DBC_resultados> options)
        : base(options)
    {
    }

    public virtual DbSet<Resultado> Resultados { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=ESILENOVO05\\SQLEXPRESS02;Database=demo_web;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resultado>(entity =>
        {
            modelBuilder.Entity<Resultado>()
            .HasKey(r => new { r.Nomina, r.Id });

            entity.Property(e => e.Antiguedad).HasColumnName("antiguedad");
            entity.Property(e => e.Cia).HasColumnName("cia");
            entity.Property(e => e.Contope)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("contope");
            entity.Property(e => e.Departamento)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("departamento");
            entity.Property(e => e.Desempenio).HasColumnType("numeric(26, 6)");
            entity.Property(e => e.Fechaingreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaingreso");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Jefe).HasColumnName("jefe");
            entity.Property(e => e.Mediatab)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("mediatab");
            entity.Property(e => e.Mediatab2)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("mediatab2");
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
            entity.Property(e => e.Normppa)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("normppa");
            entity.Property(e => e.Nvopra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("nvopra");
            entity.Property(e => e.PorcIncrementoSugerido).HasColumnType("numeric(32, 9)");
            entity.Property(e => e.PorcTabulador).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PorcentajeMinimo)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("porcentaje_minimo");
            entity.Property(e => e.PorcentajeMinimoJefe)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("porcentaje_minimo_jefe");
            entity.Property(e => e.Porcformula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("porcformula");
            entity.Property(e => e.Porctab)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("porctab");
            entity.Property(e => e.Posicion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("posicion");
            entity.Property(e => e.Ppa)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ppa");
            entity.Property(e => e.PpaAster)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ppa_aster");
            entity.Property(e => e.Pra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("pra");
            entity.Property(e => e.Puesto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("puesto");
            entity.Property(e => e.Segmento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("segmento");
            entity.Property(e => e.Sem12023).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Sem22023).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Sintope)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sintope");
            entity.Property(e => e.Sueldodiario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldodiario");
            entity.Property(e => e.Sueldomensual)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldomensual");
            entity.Property(e => e.Sueldonuevo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("sueldonuevo");
            entity.Property(e => e.Tabulador)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tabulador");
            entity.Property(e => e.Tipotab)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipotab");
            entity.Property(e => e.Tipotrab)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipotrab");
            entity.Property(e => e.Vacio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vacio");
            entity.Property(e => e.Vacio2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vacio2");
            entity.Property(e => e.JustificacionJefe)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("justificacion_jefe");
            entity.Property(e => e.JustificacionSuperJefe)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("justificacion_super_jefe");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
