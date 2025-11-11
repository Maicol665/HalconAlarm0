using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public class RolesRepositorio : IRolesRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public RolesRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

       public async Task<List<Roles>> ObtenerTodosLosRoles()
        {
            try
            {
                return await _context.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles: " + ex.Message);
            }
        }
        public async Task<Roles?> ObtenerRolPorID(Guid rolID)
        {
            try
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.RolID == rolID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el rol por ID: " + ex.Message);
            }
        }
        public async Task<bool> RegistrarRol(Roles rol)
        {
            try
            {
                await _context.Roles.AddAsync(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el rol: " + ex.Message);
            }
        }
        public async Task<bool> ActualizarRol(Roles rol)
        {
            try
            {
                _context.Roles.Update(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el rol: " + ex.Message);
            }
        }

        public async Task<bool> EliminarRol(Guid rolID)
        {
            try
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.RolID == rolID);
                if (rol == null)
                {
                    return false; // Rol no encontrado
                }
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el rol: " + ex.Message);
            }
        }
    }
}
