using System;
using HalconAlarm0.Modelos;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Contexto
{
    public class ContextoHalconAlarm0 : DbContext
    {
        public ContextoHalconAlarm0(DbContextOptions<ContextoHalconAlarm0> options) : base(options) // este es el constructor que recibe las opciones de configuración y las pasa a la clase base DbContext
        {
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) // este método se utiliza para configurar el modelo de datos
        {
            base.OnModelCreating(modelBuilder);
            // Configuración adicional si es necesario


            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.UsuarioID);
                entity.Property(e => e.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.CorreoElectronico).IsUnique();
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.ContrasenaHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContrasenaSalt).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Activo).IsRequired();
                entity.Property(e => e.FechaRegistro).IsRequired();
                entity.HasOne(e => e.Rol) // configuración de la relación con la entidad Roles 
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(e => e.RolID);

             });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RolID);
                entity.Property(e => e.NombreRol).IsRequired().HasMaxLength(50);
            });
        }        
    }
}
