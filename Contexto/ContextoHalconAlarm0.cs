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

        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<SolicitudesCotizacion> SolicitudesCotizacion { get; set; }


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

            // -----------------------------
            // CONFIGURACIÓN Contactos
            // -----------------------------
            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.HasKey(e => e.ContactoID);

                entity.Property(e => e.ContactoID)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Apellidos)
                      .HasMaxLength(100);

                entity.Property(e => e.CorreoElectronico)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(e => e.CorreoElectronico)
                      .HasDatabaseName("IX_Contactos_CorreoElectronico");

                entity.Property(e => e.Ciudad)
                      .HasMaxLength(100);

                entity.Property(e => e.Telefono)
                      .HasMaxLength(20);

                // Fecha por defecto en SQL Server
                entity.Property(e => e.FechaContacto)
                      .HasDefaultValueSql("GETDATE()");

                // FKs opcionales a Servicios y Productos
                entity.HasOne(e => e.Servicio)
                      .WithMany()
                      .HasForeignKey(e => e.ServicioID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoID)
                      .OnDelete(DeleteBehavior.Restrict);

                // CHECK para que al menos uno (ServicioID o ProductoID) sea no nulo
                entity.HasCheckConstraint("CHK_Contactos_AlMenosUno", "(ServicioID IS NOT NULL OR ProductoID IS NOT NULL)");
            });

            // -----------------------------
            // CONFIGURACIÓN SolicitudesCotizacion
            // -----------------------------
            modelBuilder.Entity<SolicitudesCotizacion>(entity =>
            {
                entity.HasKey(e => e.SolicitudID);

                entity.Property(e => e.SolicitudID)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Iniciado");

                entity.Property(e => e.FechaSolicitud)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Contacto)
                      .WithMany()
                      .HasForeignKey(e => e.ContactoID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Servicio)
                      .WithMany()
                      .HasForeignKey(e => e.ServicioID)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoID)
                      .OnDelete(DeleteBehavior.SetNull);


                // CHECK usando ToTable (requerido por EF Core 8+)
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CHK_SolicitudesCotizacion_Estado",
                        "Estado IN ('Iniciado', 'En progreso', 'Completado')"
                    );
                });

            });
            


        }
    }
}
