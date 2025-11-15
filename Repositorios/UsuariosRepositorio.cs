using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public UsuariosRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<List<Usuarios>> ObtenerTodosLosUsuarios()
        {
            try
            {
                return await _context.Usuarios
                .Include(u => u.Rol) // Incluye la información del rol asociado
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }
        }

        public async Task<Usuarios?> ObtenerUsuarioPorID(Guid id )
        {
            try
            {
                return await _context.Usuarios
                .Include(u => u.Rol) // Incluye la información del rol asociado
                .FirstOrDefaultAsync(u => u.UsuarioID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario por ID: " + ex.Message);
            }
        }
      
        public async Task<bool> RegistrarUsuario(Usuarios usuario)
        {
            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }

        public async Task<bool> ActualizarUsuario(Usuarios usuario)
        {
            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
        }

        public async Task<bool> EliminarUsuario(Guid usuarioID)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioID);
                if (usuario == null)
                {
                    return false;
                }
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario: " + ex.Message);
            }
        }
        // En UsuariosRepositorio.cs
        public async Task<Usuarios?> ObtenerUsuarioPorCorreo(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == correo);
        }

    }
}
