using HalconAlarm0.Modelos;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Contexto
{
    public class ContextoHalconAlarm0 : DbContext
    {
        public ContextoHalconAlarm0(DbContextOptions<ContextoHalconAlarm0> options)
            : base(options)
        {
        }

        // ============================
        // 🔹 TABLAS DEL SISTEMA
        // ============================
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }

        // ============================
        // 🔹 TABLAS DE SERVICIOS
        // ============================
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<ServiciosContratados> ServiciosContratados { get; set; }
        // tabla de dispositivos
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<DispositivosAsignados> DispositivosAsignados { get; set; }

        public DbSet<Productos> Productos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único en Correo Electrónico
            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.CorreoElectronico)
                .IsUnique();

            // Relación Servicios → ServiciosContratados (1:N)
            modelBuilder.Entity<ServiciosContratados>()
                .HasOne(s => s.Servicio)
                .WithMany(s => s.ServiciosContratados)
                .HasForeignKey(s => s.ServicioID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuarios → ServiciosContratados (1:N)
            modelBuilder.Entity<ServiciosContratados>()
                .HasOne<Usuarios>()
                .WithMany()
                .HasForeignKey(s => s.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispositivo>()
               .HasKey(d => d.DispositivoID);

            modelBuilder.Entity<Dispositivo>()
                .HasOne(d => d.Servicio)
                .WithMany()
                .HasForeignKey(d => d.ServicioID);

            modelBuilder.Entity<ServiciosContratados>()
                .HasKey(sc => sc.ContratoID);

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.ProductoID);

                entity.Property(e => e.ProductoID)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreProducto)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Marca)
                    .HasMaxLength(100);

                // Modelo es NVARCHAR(100)
                entity.Property(e => e.Modelo)
                    .HasMaxLength(100);

                // ImagenURL es NVARCHAR(255)
                entity.Property(e => e.ImagenURL)
                    .HasMaxLength(255);
            });



        }
    }
}
