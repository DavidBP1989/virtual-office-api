using cobach_api.Application.Dtos.Seguridad;
using cobach_api.Features.Seguridad.Interfaces;
using cobach_api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Seguridad
{
    public class SeguridadRepository : IAuthentication
    {
        readonly SiiaContext _context;
        public SeguridadRepository(SiiaContext context)
        {
            _context = context;
        }

        public async Task<UserConfig> GetUserConfig(string user)
        {
            var result = await _context.Empleados
                .Where(x => x.Usuario == user)
                .Select(u => new UserConfig
                {
                    FirstTimeLogin = u.Usuario == u.ClaveUsuario,
                    UserId = u.EmpleadoId
                }).FirstAsync();

            var allowConfirmWorkPermits = await _context.AutorizacionSolicitudes.AnyAsync(x => x.Autoriza1 == result.UserId || x.Autoriza2 == result.UserId);
            result.AllowConfirmWorkPermits = allowConfirmWorkPermits;

            var confirmedAsCampus = await _context.AutorizacionSolicitudes
                .Join(_context.CmcatalogoProyectos, a => a.IdProyecto, b => b.IdCatalogoProyecto, (a, b) => new { a.Autoriza1, b.ClaveProyecto })
                .AnyAsync(x => x.ClaveProyecto.StartsWith("02") && x.Autoriza1 == result.UserId);

            result.ShowHistory = await _context.AutorizacionSolicitudes
                .AnyAsync(x => x.AutorizaPermiso == result.UserId || (allowConfirmWorkPermits && confirmedAsCampus));

            result.ConfirmedAsCampus = confirmedAsCampus;

            return result;
        }

        public async Task<bool> Login(string user, string password)
        {
            var query = await _context.Empleados
                .Join(_context.InformacionLaborals, e => e.EmpleadoId, i => i.EmpleadoId, (e, i) => new
                {
                    e,
                    status = i.Status,
                })
                .Where(x => x.e.Usuario == user && x.e.ClaveUsuario == password && x.status == 1)
                .ToListAsync();
            return query.Any();
        }
    }
}
