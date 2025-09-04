using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Cabaña> Cabañas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Cancelacion> Cancelaciones { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cabaña>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Capacidad).IsRequired();
                entity.Property(e => e.PrecioPorNoche).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Activa).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.ImagenUrl).HasMaxLength(600);
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MetodoPago).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IdTransaccion).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.Total).IsRequired().HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Reserva)
                      .WithMany()
                      .HasForeignKey(e => e.IdReserva)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Contraseña).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Telefono).HasMaxLength(15);
                entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.Grupo).IsRequired();
            }).Entity<Usuario>().HasIndex(u => u.NombreUsuario).IsUnique();


            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Dni).IsRequired().HasMaxLength(10);

            }).Entity<Cliente>().HasIndex(u => u.Dni).IsUnique();


            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaEntrada).IsRequired();
                entity.Property(e => e.FechaSalida).IsRequired();
                entity.Property(e => e.Precio).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).IsRequired();
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.IdCliente)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Cabaña)
                      .WithMany()
                      .HasForeignKey(e => e.IdCabaña)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cancelacion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.Motivo).IsRequired();
                entity.HasOne(e => e.Reserva)
                      .WithMany()
                      .HasForeignKey(e => e.IdReserva)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
