using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class DBC_admon_sueldos : DbContext
{
    public DBC_admon_sueldos() { }

    public DBC_admon_sueldos(DbContextOptions<DBC_admon_sueldos> options)
        : base(options) { }

    public virtual DbSet<AdmonSueldo> AdmonSueldos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdmonSueldo>(entity =>
        {
            entity.HasKey(e => e.ID); // ahora EF sabe la PK
            entity.ToTable("admon_sueldos");

            entity.Property(e => e.ID).HasColumnName("ID");

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
            entity.Property(e => e.Fechaingreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaingreso");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
