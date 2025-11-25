using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class HistorialNovedadesRepositorio : IHistorialNovedadesRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public HistorialNovedadesRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task CrearHistorialAsync(HistorialNovedades historial)
        {
            _context.HistorialNovedades.Add(historial);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistorialNovedades>> ObtenerPorNovedadAsync(Guid novedadID)
        {
            return await _context.HistorialNovedades
                .Where(h => h.NovedadID == novedadID)
                .OrderByDescending(h => h.FechaCambio)
                .ToListAsync();
        }
    }
}
