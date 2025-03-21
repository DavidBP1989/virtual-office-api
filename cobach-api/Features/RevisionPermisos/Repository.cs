using cobach_api.Application.Dtos.RevisionPermisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Persistence.Models;
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

                int? proyectoId = _context.InformacionLaborals.First(x => x.EmpleadoId == corteTiempo.EmpleadoId && x.StatusBorrado == 0 && x.TurnoxCentroDeTrabajoId == corteTiempo.TurnoCentroTrabajoId)?.IdCatalogoProyecto;
                var statusFirma = StatusFirma(proyectoId.GetValueOrDefault());
                if (statusFirma == PermisoStatusFirma.SinAutorizacion)
                {
                    return false;
                }
                int statusPermiso = statusFirma == PermisoStatusFirma.AutorizaAmbasFirmas ? 2 : (int)statusFirma;

                corteTiempo.EstatusPermiso = statusPermiso;
                corteTiempo.EstatusFirma = _user.GetCurrentUser();
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                var permisoEconomico = await _context.PermisoEconomicos.FindAsync(permisoLaboralId);
                if (permisoEconomico is null)
                    return false;

                int? proyectoId = _context.InformacionLaborals.First(x => x.EmpleadoId == permisoEconomico.EmpleadoId && x.StatusBorrado == 0 && x.TurnoxCentroDeTrabajoId == permisoEconomico.TurnoCentroTrabajoId)?.IdCatalogoProyecto;
                var statusFirma = StatusFirma(proyectoId.GetValueOrDefault());
                if (statusFirma == PermisoStatusFirma.SinAutorizacion)
                {
                    return false;
                }
                int statusPermiso = statusFirma == PermisoStatusFirma.AutorizaAmbasFirmas ? 2 : (int)statusFirma;

                permisoEconomico.EstatusPermiso = statusPermiso;
                permisoEconomico.EstatusFirma = _user.GetCurrentUser();
                _context.Update(permisoEconomico);
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }

        public async Task<PermisosLaborales> ObtenerPermisosLaboralesPendientesPorRevisar(List<AutorizacionProyectos> autorizacionProyectos)
        {
            var result = new PermisosLaborales
            {
                CortesTiempo = new List<PermisoLaboral>(),
                PermisosEconomicos = new List<PermisoLaboral>()
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
                .Join(_context.TurnosxCentrosDeTrabajos, t => t.i.TurnoxCentroDeTrabajoId, tct => tct.TurnoxCentroDeTrabajoId, (cte, tct) => new { cte, tct })
                .Where(
                    x => proyecto.ProyectoId == x.cte.i.IdCatalogoProyecto &&
                    x.cte.cte.ct.CentroDeTrabajoId == x.tct.CentroDeTrabajoId &&
                    x.cte.cte.ct.TurnoCentroTrabajoId == x.cte.i.TurnoxCentroDeTrabajoId &&
                    x.cte.cte.ct.EstatusPermiso == proyecto.EstatusPermiso
                )
                .Select(x => new PermisoLaboral
                {
                    PermisoId = x.cte.cte.ct.Id,
                    Empleado = $"{x.cte.cte.Nombres} {x.cte.cte.PrimerApellido} {x.cte.cte.SegundoApellido}",
                    CentroDeTrabajo =
                        _context.TurnosxCentrosDeTrabajos
                        .Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                        {
                            t.TurnoxCentroDeTrabajoId,
                            t.Turno,
                            c.Nombre,
                            c.Clave
                        })
                        .Where(w => w.TurnoxCentroDeTrabajoId == x.cte.cte.ct.TurnoCentroTrabajoId)
                        .Select(s =>
                            s.Clave.Substring(0, 2) == "03"
                            ? $"{s.Nombre.Substring(45, 10)}, {(s.Turno == 0 ? "TM" : "TV")}"
                            : $"{s.Nombre}, {(s.Turno == 0 ? "TM" : "TV")}"
                        )
                        .FirstOrDefault(),
                    FechaRegisto = x.cte.cte.ct.FechaRegisto.GetValueOrDefault(),
                    FechaSolicitud = x.cte.cte.ct.FechaSolicitud.GetValueOrDefault(),
                    HoraSalida = x.cte.cte.ct.HoraSalida.GetValueOrDefault(),
                    TiempoEstimado = x.cte.cte.ct.TiempoEstimado.GetValueOrDefault(),
                    Comentario = x.cte.cte.ct.Comentario,
                    Estatus = x.cte.cte.ct.EstatusPermiso.GetValueOrDefault(),
                    TiempoReal = _context.CorteTiempos
                        .Where(w => w.EmpleadoId == x.cte.cte.EmpleadoId && w.EstatusPermiso == (int)EstatusPermisos.Confirmado && w.Comprobo.HasValue && !w.Comprobo.Value)
                        .Select(s => s.TiempoReal)
                        .Sum(),
                    TiempoLimite = _context.CatalogoPermisosLaborales
                        .First(w => w.Id == (int)TipoPermisosLaborales.CorteTiempo && w.Activo.HasValue && w.Activo.Value).TiempoPermitido.GetValueOrDefault()
                })
                .ToListAsync();

                var permisoEconomico = await _context.PermisoEconomicos
                    .Join(_context.Empleados, pe => pe.EmpleadoId, e => e.EmpleadoId, (pe, e) => new
                    {
                        pe,
                        e.EmpleadoId,
                        e.Nombres,
                        e.PrimerApellido,
                        e.SegundoApellido
                    })
                    .Join(_context.InformacionLaborals, pee => pee.EmpleadoId, i => i.EmpleadoId, (pee, i) => new { pee, i })
                    .Join(_context.TurnosxCentrosDeTrabajos, t => t.i.TurnoxCentroDeTrabajoId, tct => tct.TurnoxCentroDeTrabajoId, (cte, tct) => new { cte, tct })
                    .Where(x =>
                        proyecto.ProyectoId == x.cte.i.IdCatalogoProyecto &&
                        x.cte.pee.pe.CentroDeTrabajoId == x.tct.CentroDeTrabajoId &&
                        x.cte.pee.pe.TurnoCentroTrabajoId == x.cte.i.TurnoxCentroDeTrabajoId &&
                        x.cte.pee.pe.EstatusPermiso == proyecto.EstatusPermiso
                    )
                    .Select(x => new PermisoLaboral
                    {
                        PermisoId = x.cte.pee.pe.Id,
                        Empleado = $"{x.cte.pee.Nombres} {x.cte.pee.PrimerApellido} {x.cte.pee.SegundoApellido}",
                        CentroDeTrabajo =
                            _context.TurnosxCentrosDeTrabajos
                            .Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                            {
                                t.TurnoxCentroDeTrabajoId,
                                t.Turno,
                                c.Nombre,
                                c.Clave
                            })
                            .Where(w => w.TurnoxCentroDeTrabajoId == x.cte.pee.pe.TurnoCentroTrabajoId)
                            .Select(s =>
                                s.Clave.Substring(0, 2) == "03"
                                ? $"{s.Nombre.Substring(45, 10)}, {(s.Turno == 0 ? "TM" : "TV")}"
                                : $"{s.Nombre}, {(s.Turno == 0 ? "TM" : "TV")}"
                            )
                            .FirstOrDefault(),
                        FechaRegisto = x.cte.pee.pe.FechaRegistro.GetValueOrDefault(),
                        FechaSolicitud = x.cte.pee.pe.FechaSolicitud.GetValueOrDefault(),
                        Comentario = x.cte.pee.pe.Comentario,
                        Estatus = x.cte.pee.pe.EstatusPermiso.GetValueOrDefault(),
                        LapsoPermisoDiasHabiles = x.cte.pee.pe.LapsoPermisoDiasHabiles,
                        ComentarioDias = x.cte.pee.pe.ComentarioDias ?? "",
                        ConGoceSueldo = x.cte.pee.pe.ConGoceSueldo,
                        DiasReales = _context.PermisoEconomicos
                            .Where(w => w.EmpleadoId == x.cte.pee.EmpleadoId && w.EstatusPermiso == (int)EstatusPermisos.Confirmado)
                            .Select(s => s.LapsoPermisoDiasHabiles)
                            .Sum(),
                        TiempoLimite = _context.CatalogoPermisosLaborales
                            .First(w => w.Id == (int)TipoPermisosLaborales.PermisoEconomico && w.Activo.HasValue && w.Activo.Value).TiempoPermitido.GetValueOrDefault()
                    })
                    .ToListAsync();

                result.CortesTiempo.AddRange(corteTiempo);
                result.PermisosEconomicos.AddRange(permisoEconomico);
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

            workPermits.AddRange(await _context.AutorizacionSolicitudes
                .Where(x => x.AutorizaPermiso == _user.GetCurrentUser())
                .Select(x => new AutorizacionProyectos
                {
                    ProyectoId = x.IdProyecto,
                    EstatusPermiso = 2
                }).ToListAsync());

            return workPermits;
        }

        public async Task<bool> AutorizarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, int tiempoReal, bool justificanteISSSTE)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                corteTiempo.EstatusPermiso = (int)EstatusPermisos.Confirmado;
                corteTiempo.TiempoReal = tiempoReal;
                corteTiempo.Comprobo = justificanteISSSTE;
                corteTiempo.EstatusFirma = _user.GetCurrentUser();
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                var permisoEconomico = await _context.PermisoEconomicos.FindAsync(permisoLaboralId);
                if (permisoEconomico is null)
                    return false;

                permisoEconomico.EstatusPermiso = (int)EstatusPermisos.Confirmado;
                permisoEconomico.EstatusFirma = _user.GetCurrentUser();
                _context.Update(permisoEconomico);
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }

        public async Task<bool> RechazarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, string motivo)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                corteTiempo.EstatusPermiso = (int)EstatusPermisos.Rechazado;
                corteTiempo.MotivoRechazo = motivo;
                corteTiempo.EstatusFirma = _user.GetCurrentUser();
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                var permisoEconomico = await _context.PermisoEconomicos.FindAsync(permisoLaboralId);
                if (permisoEconomico is null)
                    return false;

                permisoEconomico.EstatusPermiso = (int)EstatusPermisos.Rechazado;
                permisoEconomico.MotivoRechazo = motivo;
                permisoEconomico.EstatusFirma = _user.GetCurrentUser();
                _context.Update(permisoEconomico);
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }

        public async Task<bool> EliminarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, string motivo)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                corteTiempo.MotivoEliminacion = motivo;
                corteTiempo.EstatusPermiso = (int)EstatusPermisos.Eliminado;
                _context.Update(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                var permisoEconomico = await _context.PermisoEconomicos.FindAsync(permisoLaboralId);
                if (permisoEconomico is null)
                    return false;

                permisoEconomico.MotivoEliminacion = motivo;
                permisoEconomico.EstatusPermiso = (int)EstatusPermisos.Eliminado;
                result = await _context.SaveChangesAsync() > 0;
            }
            
            return result;
        }

        public async Task<bool> EliminarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId)
        {
            bool result = false;
            if (tipoPermiso == TipoPermisosLaborales.CorteTiempo)
            {
                var corteTiempo = await _context.CorteTiempos.FindAsync(permisoLaboralId);
                if (corteTiempo is null)
                    return false;

                _context.Attach(corteTiempo);
                _context.Remove(corteTiempo);
                result = await _context.SaveChangesAsync() > 0;
            }
            else if (tipoPermiso == TipoPermisosLaborales.PermisoEconomico)
            {
                var permisoEconomico = await _context.PermisoEconomicos.FindAsync(permisoLaboralId);
                if (permisoEconomico is null)
                    return false;

                _context.Attach(permisoEconomico);
                _context.Remove(permisoEconomico);
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }
    }
}
