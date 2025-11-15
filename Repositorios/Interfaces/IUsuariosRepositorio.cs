using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IUsuariosRepositorio 
    {
        public Task<List<Modelos.Usuarios>> ObtenerTodosLosUsuarios ();
        public Task<Modelos.Usuarios> ObtenerUsuarioPorID (Guid usuarioID);
        public Task<bool> RegistrarUsuario (Modelos.Usuarios usuarios);
        public Task<bool> ActualizarUsuario (Modelos.Usuarios usuario);
        public Task<bool> EliminarUsuario (Guid usuarioID);
        // En IUsuariosRepositorio.cs
        Task<Usuarios?> ObtenerUsuarioPorCorreo(string correo);

    }
}
