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

            modelBuilder.Entity<Reserva>(e =>
            {
                e.Property(r => r.Total).HasColumnType("decimal(10,2)");

                e.HasOne(r => r.Cabaña)
                 .WithMany()
                 .HasForeignKey(r => r.IdCabaña)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Cliente)
                 .WithMany()
                 .HasForeignKey(r => r.IdCliente)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Cancelacion)
                 .WithOne(c => c.Reserva)
                 .HasForeignKey<Cancelacion>(c => c.ReservaId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(r => new { r.IdCabaña, r.FechaEntrada, r.FechaSalida });
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

                e.HasIndex(x => new { x.Año, x.Tipo })
                 .IsUnique()
                 .HasFilter("[Tipo] = 0 AND [IsActive] = 1");

                e.HasIndex(x => new { x.IdCabaña, x.Año, x.Tipo })
                 .IsUnique()
                 .HasFilter("[Tipo] = 1 AND [IsActive] = 1"); 

                e.HasIndex(x => new { x.IdCabaña, x.Año, x.Mes, x.Tipo })
                 .IsUnique()
                 .HasFilter("[Tipo] = 2 AND [IsActive] = 1"); 
            });

            modelBuilder.Entity<Cancelacion>(e =>
            {
                e.HasIndex(c => c.ReservaId).IsUnique();
            });
        }


    }
}






