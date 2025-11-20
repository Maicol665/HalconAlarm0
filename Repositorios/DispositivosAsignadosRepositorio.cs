using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class DispositivosAsignadosRepositorio : IDispositivosAsignadosRepositorio
    {
        private readonly ContextoHalconAlarm0 _contexto;

        public DispositivosAsignadosRepositorio(ContextoHalconAlarm0 contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<DispositivosAsignados>> ObtenerTodosAsync()
        {
            return await _contexto.DispositivosAsignados.ToListAsync();
        }

        public async Task<DispositivosAsignados?> ObtenerPorIdAsync(Guid id)
        {
            return await _contexto.DispositivosAsignados
                .FirstOrDefaultAsync(x => x.AsignacionID == id);
        }

        public async Task CrearAsync(DispositivosAsignados entidad)
        {
            await _contexto.DispositivosAsignados.AddAsync(entidad);
        }

        public async Task GuardarAsync()
        {
            await _contexto.SaveChangesAsync();
        }
        public async Task<IEnumerable<DispositivosAsignados>> ObtenerPorUsuario(Guid usuarioId)
        {
            return await _contexto.DispositivosAsignados
                .Where(x => x.UsuarioID == usuarioId)
                .Include(x => x.Dispositivo)  // TRAE información del dispositivo
                .ToListAsync();
        }


    }
}
