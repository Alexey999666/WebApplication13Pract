using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication13Pract.Models;

public partial class SalonDb2Context : DbContext
{
    public SalonDb2Context()
    {
    }

    public SalonDb2Context(DbContextOptions<SalonDb2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress; Database=SalonDB2; User=исп-41; Password=1234567890; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.CardNumber).HasName("PK__Clients__A4E9FFE80650AD36");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.IdclientServices);

            entity.Property(e => e.IdclientServices).HasColumnName("IDClientServices");
            entity.Property(e => e.AppointmentDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientServices_Clients");

            entity.HasOne(d => d.Service).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientServices_Services");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Services__A25C5AA66752C062");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
