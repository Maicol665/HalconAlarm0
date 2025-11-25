using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class SolicitudesCotizacionRepositorio : ISolicitudesCotizacionRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public SolicitudesCotizacionRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SolicitudesCotizacion>> ObtenerTodasAsync()
        {
            return await _context.SolicitudesCotizacion
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SolicitudesCotizacion?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.SolicitudesCotizacion
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SolicitudID == id);
        }

        public async Task<bool> ActualizarEstadoAsync(Guid id, string nuevoEstado)
        {
            var solicitud = await _context.SolicitudesCotizacion.FindAsync(id);

            if (solicitud == null)
                return false;

            solicitud.Estado = nuevoEstado;

            _context.SolicitudesCotizacion.Update(solicitud);
            await _context.SaveChangesAsync();

            return true;
        }

        // === NUEVO: crear solicitud ===
        public async Task<SolicitudesCotizacion> CrearSolicitudAsync(SolicitudesCotizacion solicitud)
        {
            if (solicitud == null) throw new ArgumentNullException(nameof(solicitud));

            // Asegúrate de fijar fecha / estado por si acaso
            if (solicitud.FechaSolicitud == default) solicitud.FechaSolicitud = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(solicitud.Estado)) solicitud.Estado = "Iniciado";

            await _context.SolicitudesCotizacion.AddAsync(solicitud);
            await _context.SaveChangesAsync();

            return solicitud;
        }
    }
}
