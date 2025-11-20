using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class ServiciosContratadosRepositorio : IServiciosContratadosRepositorio
    {
        private readonly ContextoHalconAlarm0 _context;

        public ServiciosContratadosRepositorio(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        public async Task<bool> ExisteContrato(Guid usuarioId, Guid servicioId)
        {
            return await _context.ServiciosContratados
                .AnyAsync(s => s.UsuarioID == usuarioId && s.ServicioID == servicioId);
        }

        public async Task<bool> Registrar(ServiciosContratados contrato)
        {
            await _context.ServiciosContratados.AddAsync(contrato);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
