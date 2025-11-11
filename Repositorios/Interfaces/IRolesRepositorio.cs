namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IRolesRepositorio
    {
     public Task<List<Modelos.Roles>> ObtenerTodosLosRoles ();
     public Task<Modelos.Roles> ObtenerRolPorID (Guid rolID);
     public Task<bool> RegistrarRol (Modelos.Roles rol);
     public Task<bool> ActualizarRol (Modelos.Roles rol);
    public Task<bool> EliminarRol (Guid rolID);
    }
}
