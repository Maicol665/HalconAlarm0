using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class DispositivoRepositorio : IDispositivoRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public DispositivoRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<Dispositivo> CrearDispositivo(Dispositivo dispositivo)
        {
            _context.Dispositivos.Add(dispositivo);
            await _context.SaveChangesAsync();
            return dispositivo;
        }

        public async Task<Dispositivo?> ObtenerPorId(Guid dispositivoId)
        {
            return await _context.Dispositivos.FirstOrDefaultAsync(d => d.DispositivoID == dispositivoId);
        }

        public async Task<IEnumerable<Dispositivo>> ObtenerPorServicio(Guid servicioId)
        {
            return await _context.Dispositivos
                .Where(d => d.ServicioID == servicioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dispositivo>> ObtenerPorUsuario(Guid usuarioId)
        {
            return await _context.ServiciosContratados
                .Where(sc => sc.UsuarioID == usuarioId)
                .Join(_context.Dispositivos,
                      sc => sc.ServicioID,
                      d => d.ServicioID,
                      (sc, d) => d)
                .ToListAsync();
        }

        public async Task<bool> Actualizar(Dispositivo dispositivo)
        {
            _context.Dispositivos.Update(dispositivo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(Guid id)
        {
            var dispositivo = await ObtenerPorId(id);
            if (dispositivo == null) return false;

            _context.Dispositivos.Remove(dispositivo);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
