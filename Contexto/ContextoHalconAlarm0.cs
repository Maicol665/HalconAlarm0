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
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<DispositivosAsignados> DispositivosAsignados { get; set; }

        public DbSet<Productos> Productos { get; set; } = null!;

        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<SolicitudesCotizacion> SolicitudesCotizacion { get; set; }

        // ============================
        // 🔹 NOVEDADES
        // ============================
        public DbSet<Novedades> Novedades { get; set; }
        public DbSet<TiposNovedad> TiposNovedad { get; set; }
        public DbSet<HistorialNovedades> HistorialNovedades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================================
            // USUARIOS
            // ============================================================
            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.CorreoElectronico)
                .IsUnique();

            // ============================================================
            // SERVICIOS CONTRATADOS
            // ============================================================
            modelBuilder.Entity<ServiciosContratados>()
                .HasOne(s => s.Servicio)
                .WithMany(s => s.ServiciosContratados)
                .HasForeignKey(s => s.ServicioID)
                .OnDelete(DeleteBehavior.Restrict);

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

            // ============================================================
            // PRODUCTOS
            // ============================================================
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

                entity.Property(e => e.Modelo)
                    .HasMaxLength(100);

                entity.Property(e => e.ImagenURL)
                    .HasMaxLength(255);
            });

            // ============================================================
            // CONTACTOS
            // ============================================================
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

                entity.Property(e => e.FechaContacto)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Servicio)
                      .WithMany()
                      .HasForeignKey(e => e.ServicioID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.ProductoID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasCheckConstraint("CHK_Contactos_AlMenosUno",
                    "(ServicioID IS NOT NULL OR ProductoID IS NOT NULL)");
            });

            // ============================================================
            // SOLICITUDES DE COTIZACIÓN
            // ============================================================
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

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CHK_SolicitudesCotizacion_Estado",
                        "Estado IN ('Iniciado', 'En progreso', 'Completado')"
                    );
                });
            });

            // ============================================================
            // TIPOS DE NOVEDAD
            // ============================================================
            modelBuilder.Entity<TiposNovedad>(entity =>
            {
                entity.HasKey(e => e.TipoNovedadID);

                entity.Property(e => e.NombreTipo)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(e => e.NombreTipo)
                      .IsUnique();
            });

            // ============================================================
            // NOVEDADES
            // ============================================================
            modelBuilder.Entity<Novedades>(entity =>
            {
                entity.HasKey(e => e.NovedadID);

                entity.Property(e => e.FechaNovedad)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.Estado)
                      .HasMaxLength(50)
                      .HasDefaultValue("Abierto");

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CHK_Novedades_Estado",
                        "Estado IN ('Abierto','En Progreso','Cerrado')"
                    );
                });

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TipoNovedad)
                      .WithMany()
                      .HasForeignKey(e => e.TipoNovedadID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================================
            // HISTORIAL DE NOVEDADES
            // ============================================================
            modelBuilder.Entity<HistorialNovedades>(entity =>
            {
                entity.HasKey(e => e.HistorialID);

                entity.Property(e => e.FechaCambio)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Novedad)
                      .WithMany(n => n.Historial)
                      .HasForeignKey(e => e.NovedadID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
