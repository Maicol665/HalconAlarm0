using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class NovedadesRepositorio : INovedadesRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public NovedadesRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<Novedades> CrearNovedadAsync(Novedades novedad)
        {
            _context.Novedades.Add(novedad);
            await _context.SaveChangesAsync();
            return novedad;
        }

        public async Task<Novedades> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Novedades
                .Include(n => n.Usuario)
                .Include(n => n.TipoNovedad)
                .FirstOrDefaultAsync(n => n.NovedadID == id);
        }

        public async Task<IEnumerable<Novedades>> ObtenerTodasAsync()
        {
            return await _context.Novedades
                .Include(n => n.TipoNovedad)
                .Include(n => n.Usuario)
                .ToListAsync();
        }

        public async Task<bool> ActualizarEstadoAsync(Guid novedadID, string nuevoEstado, string comentario)
        {
            var novedad = await _context.Novedades.FindAsync(novedadID);
            if (novedad == null) return false;

            string estadoAnterior = novedad.Estado;
            novedad.Estado = nuevoEstado;

            _context.HistorialNovedades.Add(new HistorialNovedades
            {
                NovedadID = novedadID,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = nuevoEstado,
                Comentarios = comentario,
                FechaCambio = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
