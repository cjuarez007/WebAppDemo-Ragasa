using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class DBC_porcentajes_estandar : DbContext
{
    public DBC_porcentajes_estandar()
    {
    }

    public DBC_porcentajes_estandar(DbContextOptions<DBC_porcentajes_estandar> options)
        : base(options)
    {
    }

    public virtual DbSet<PorcentajesEstandar> PorcentajesEstandars { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-KC7649P\\ALEXSIS007;Database=demo_web;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PorcentajesEstandar>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("porcentajes_estandar");

            entity.Property(e => e.Valor)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valor");
            entity.Property(e => e.Variable)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("variable");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
