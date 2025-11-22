using HalconAlarm0.Contexto;
using HalconAlarm0.DTOs.Contactos;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class ContactosRepositorio : IContactosRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public ContactosRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<Contacto> RegistrarContactoAsync(RegistrarContactoDTO dto)
        {
            // Validación mínima en repositorio (puedes moverla al servicio/ controlador)
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("Nombre es obligatorio", nameof(dto.Nombre));
            if (string.IsNullOrWhiteSpace(dto.CorreoElectronico))
                throw new ArgumentException("CorreoElectronico es obligatorio", nameof(dto.CorreoElectronico));
            if (dto.ServicioID == null && dto.ProductoID == null)
                throw new ArgumentException("Debe especificar ServicioID o ProductoID");

            var contacto = new Contacto
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                CorreoElectronico = dto.CorreoElectronico,
                Ciudad = dto.Ciudad,
                Telefono = dto.Telefono,
                ServicioID = dto.ServicioID,
                ProductoID = dto.ProductoID
                // FechaContacto se toma por defecto
            };

            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();

            return contacto;
        }

        public async Task<IEnumerable<Contacto>> ListarContactosAsync(int? take = null, int? skip = null)
        {
            IQueryable<Contacto> q = _context.Contactos
                                             .Include(c => c.Servicio)
                                             .Include(c => c.Producto)
                                             .OrderByDescending(c => c.FechaContacto);

            if (skip.HasValue) q = q.Skip(skip.Value);
            if (take.HasValue) q = q.Take(take.Value);

            return await q.ToListAsync();
        }

        public async Task<Contacto?> ObtenerPorIdAsync(Guid contactoId)
        {
            return await _context.Contactos
                                 .Include(c => c.Servicio)
                                 .Include(c => c.Producto)
                                 .FirstOrDefaultAsync(c => c.ContactoID == contactoId);
        }

        public async Task<bool> EliminarContactoAsync(Guid id)
        {
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null)
                return false;

            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
