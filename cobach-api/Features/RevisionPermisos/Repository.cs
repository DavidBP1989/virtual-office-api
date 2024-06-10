using cobach_api.Application.Dtos.RevisionPermisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.RevisionPermisos
{
    public class Repository : IRevisionPermisos
    {
        readonly SiiaContext _context;
        readonly IUserService _user;
        public Repository(SiiaContext context, IUserService user)
        {
            _context = context;
            _user = user;
        }

        PermisoStatusFirma StatusFirma(int proyectoId)
        {
            string user = _user.GetCurrentUser();

            if (_context.AutorizacionSolicitudes.Any(x => x.Autoriza1 == user && x.Autoriza2 == user && x.IdProyecto == proyectoId))
                return PermisoStatusFirma.AutorizaAmbasFirmas;
            else if (_context.AutorizacionSolicitudes.Any(x => x.Autoriza2 == user && x.IdProyecto == proyectoId))
                return PermisoStatusFirma.AutorizaPrimeraFirma;
            else if (_context.AutorizacionSolicitudes.Any(x => x.Autoriza1 == user && x.IdProyecto == proyectoId))
                return PermisoStatusFirma.AutorizaSegundaFirma;
            else return PermisoStatusFirma.SinAutorizacion;
        }

        public async Task<bool> ActualizarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                int? proyectoId = _context.InformacionLaborals.First(x => x.EmpleadoId == corteTiempo.EmpleadoId)?.IdCatalogoProyecto;
                var statusFirma = StatusFirma(proyectoId.GetValueOrDefault());
                int statusPermiso = statusFirma == PermisoStatusFirma.AutorizaAmbasFirmas ? 2 : (int)statusFirma;

                corteTiempo.EstatusPermiso = statusPermiso;
                corteTiempo.EstatusFirma = _user.GetCurrentUser();
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                
            }

            return result;
        }

        public async Task<PermisosLaborales> ObtenerPermisosLaboralesPendientesPorRevisar(List<AutorizacionProyectos> autorizacionProyectos)
        {
            var result = new PermisosLaborales
            {
                CortesTiempo = new List<PermisoLaboral>()
            };

            foreach (var proyecto in autorizacionProyectos)
            {
                var corteTiempo = await _context.CorteTiempos
                .Join(_context.Empleados, ct => ct.EmpleadoId, e => e.EmpleadoId, (ct, e) => new
                {
                    ct,
                    e.EmpleadoId,
                    e.Nombres,
                    e.PrimerApellido,
                    e.SegundoApellido
                })
                .Join(_context.InformacionLaborals, cte => cte.EmpleadoId, i => i.EmpleadoId, (cte, i) => new { cte, i })
                .Where(
                    x => proyecto.ProyectoId == x.i.IdCatalogoProyecto &&
                    x.cte.ct.EstatusPermiso == proyecto.EstatusPermiso
                )
                .Select(x => new PermisoLaboral
                {
                    PermisoId = x.cte.ct.Id,
                    Empleado = $"{x.cte.Nombres} {x.cte.PrimerApellido} {x.cte.SegundoApellido}",
                    CentroDeTrabajo =
                    _context.TurnosxCentrosDeTrabajos
                    .Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                    {
                        t.TurnoxCentroDeTrabajoId,
                        t.Turno,
                        c.Nombre,
                        c.Clave
                    })
                    .Where(x => x.TurnoxCentroDeTrabajoId == x.TurnoxCentroDeTrabajoId)
                    .Select(x =>
                        x.Clave.Substring(0, 2) == "03"
                        ? $"{x.Nombre.Substring(45, 10)}, {(x.Turno == 0 ? "TM" : "TV")}"
                        : (x.Turno == 0 ? "TM" : "TV")
                    )
                    .FirstOrDefault(),
                    FechaRegisto = x.cte.ct.FechaRegisto.GetValueOrDefault(),
                    FechaSolicitud = x.cte.ct.FechaSolicitud.GetValueOrDefault(),
                    HoraSalida = x.cte.ct.HoraSalida.GetValueOrDefault(),
                    TiempoEstimado = x.cte.ct.TiempoEstimado.GetValueOrDefault(),
                    Comentario = x.cte.ct.Comentario,
                    Estatus = x.cte.ct.EstatusPermiso.GetValueOrDefault(),
                }).ToListAsync();

                result.CortesTiempo.AddRange(corteTiempo);
            }

            return result;
        }

        public async Task<List<AutorizacionProyectos>> ObtenerAutorizacionProyectos()
        {
            var workPermits = new List<AutorizacionProyectos>();

            int[] proyectsId = await _context.AutorizacionSolicitudes
                .Where(x => x.Autoriza1 == _user.GetCurrentUser() || x.Autoriza2 == _user.GetCurrentUser())
                .Select(x => x.IdProyecto)
                .ToArrayAsync();

            int statusFirma = 0;
            foreach (var proyect in proyectsId)
            {
                statusFirma = StatusFirma(proyect) == PermisoStatusFirma.AutorizaSegundaFirma ? 1 : 0;
                workPermits.Add(new AutorizacionProyectos
                {
                    ProyectoId = proyect,
                    EstatusPermiso = statusFirma
                });
            }

            bool authorizePermissions = _context.AutorizacionSolicitudes.Any(x => x.AutorizaPermiso == _user.GetCurrentUser());
            if (authorizePermissions)
            {
                workPermits.AddRange(await _context.AutorizacionSolicitudes
                    .Select(x => new AutorizacionProyectos
                    {
                        ProyectoId = x.IdProyecto,
                        EstatusPermiso = 2
                    }).ToListAsync());
            }

            return workPermits;
        }

        public async Task<bool> AutorizarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, int tiempoReal)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                corteTiempo.EstatusPermiso = 4;
                corteTiempo.TiempoReal = tiempoReal;
                corteTiempo.EstatusFirma = _user.GetCurrentUser();
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {

            }

            return result;
        }
    }
}
