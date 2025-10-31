using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Cabaña> Cabañas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }
        public DbSet<Cancelacion> Cancelaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            }).Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Cabaña>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Capacidad).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PrecioPorNoche).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaEntrada).IsRequired();
                entity.Property(e => e.FechaSalida).IsRequired();
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.IdCliente)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Cabaña)
                      .WithMany()
                      .HasForeignKey(e => e.IdCabaña)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Dni).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(15);
            });

            modelBuilder.Entity<Objetivo>(e =>
            {
                e.Property(x => x.MetaOcupacion).HasColumnType("decimal(5,2)");

                e.HasIndex(o => o.Año)
                    .HasFilter("[Tipo] = 0 AND [Mes] IS NULL AND [IdCabaña] IS NULL")
                    .IsUnique()
                    .HasDatabaseName("UX_Objetivos_General_Año");

                e.HasIndex(o => new { o.IdCabaña, o.Año })
                    .HasFilter("[Tipo] = 1 AND [Mes] IS NULL")
                    .IsUnique()
                    .HasDatabaseName("UX_Objetivos_Anual_Cabaña_Año");

                e.HasIndex(o => new { o.IdCabaña, o.Año, o.Mes })
                    .HasFilter("[Tipo] = 2")
                    .IsUnique()
                    .HasDatabaseName("UX_Objetivos_Mensual_Cabaña_AñoMes");
            });


            modelBuilder.Entity<Cancelacion>(e =>
            {
                e.HasIndex(x => new { x.ReservaId, x.Fecha });
                e.HasOne(x => x.Reserva)
                 .WithOne(r => r.Cancelacion)
                 .HasForeignKey<Cancelacion>(x => x.ReservaId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}






