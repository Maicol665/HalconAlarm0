using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class ServiciosRepositorio : IServiciosRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public ServiciosRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicios>> ObtenerTodos()
        {
            try
            {
                return await _context.Servicios.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener servicios: " + ex.Message);
            }
        }

        public async Task<Servicios?> ObtenerPorId(Guid servicioId)
        {
            try
            {
                return await _context.Servicios.FirstOrDefaultAsync(s => s.ServicioID == servicioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el servicio: " + ex.Message);
            }
        }

        public async Task<Servicios> Crear(Servicios servicio)
        {
            try
            {
                servicio.ServicioID = Guid.NewGuid();
                servicio.Activo = true;
                
                await _context.Servicios.AddAsync(servicio);
                await _context.SaveChangesAsync();
                
                return servicio;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el servicio: " + ex.Message);
            }
        }

        public async Task<bool> Actualizar(Servicios servicio)
        {
            try
            {
                _context.Servicios.Update(servicio);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el servicio: " + ex.Message);
            }
        }

        public async Task<bool> Actualizar(Guid servicioId, string nombreServicio, string? tipoServicio, string? descripcion, string? beneficios, bool activo)
        {
            try
            {
                var servicio = await ObtenerPorId(servicioId);
                if (servicio == null)
                    return false;

                servicio.NombreServicio = nombreServicio;
                servicio.TipoServicio = tipoServicio;
                servicio.Descripcion = descripcion;
                servicio.Beneficios = beneficios;
                servicio.Activo = activo;

                _context.Servicios.Update(servicio);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el servicio: " + ex.Message);
            }
        }

        public async Task<bool> ActualizarEstado(Guid servicioId, bool activo)
        {
            try
            {
                var servicio = await ObtenerPorId(servicioId);
                if (servicio == null)
                    return false;

                servicio.Activo = activo;
                _context.Servicios.Update(servicio);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado del servicio: " + ex.Message);
            }
        }

        public async Task<bool> Eliminar(Guid servicioId)
        {
            try
            {
                var servicio = await ObtenerPorId(servicioId);
                if (servicio == null)
                    return false;

                _context.Servicios.Remove(servicio);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el servicio: " + ex.Message);
            }
        }

        public async Task<bool> TieneContratos(Guid servicioId)
        {
            try
            {
                return await _context.ServiciosContratados.AnyAsync(c => c.ServicioID == servicioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar contratos: " + ex.Message);
            }
        }
    }
}
