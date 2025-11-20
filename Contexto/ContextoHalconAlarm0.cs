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
        }
    }
}
