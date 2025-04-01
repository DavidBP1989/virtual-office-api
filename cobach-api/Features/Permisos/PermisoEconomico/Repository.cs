using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos.PermisoEconomico
{
    public class Repository : IPermisoEconomico
    {
        readonly SiiaContext _context;
        public Repository(SiiaContext context)
        {
            _context = context;
        }
        public async Task<PermisoEconomicoResponse> GetPermisoEconomicoAsync(string empleadoId, CancellationToken cancellationToken, List<int>? centrosTrabajo = null)
        {
            int year = DateTime.Now.Year;
            var pe = await _context.PermisoEconomicos
                .Where(x =>
                    x.EmpleadoId == empleadoId &&
                    x.FechaRegistro.HasValue && x.FechaSolicitud.HasValue &&
                    (
                        x.FechaRegistro.Value.Year == year || x.FechaSolicitud.Value.Year == year
                    ) &&
                    x.EstatusPermiso != (int)EstatusPermisos.Eliminado &&
                    (centrosTrabajo == null || (x.CentroDeTrabajoId.HasValue && centrosTrabajo.Contains(x.CentroDeTrabajoId.Value)))
            )
            .ToListAsync(cancellationToken);

            var response = new PermisoEconomicoResponse
            {
                PermisosLimite = _context.CatalogoPermisosLaborales
                    .First(x => x.Id == (int)TipoPermisosLaborales.PermisoEconomico && x.Activo.HasValue && x.Activo.Value)
                    ?.TiempoPermitido,
                PermisosAceptados = pe
                    .Where(x => x.EstatusPermiso == (int)EstatusPermisos.Confirmado || x.EstatusPermiso == (int)EstatusPermisos.Especial)
                    .Select(s  => s.LapsoPermisoDiasHabiles)
                    .DefaultIfEmpty(0)
                    .Sum(),
                PermisoEconomicoPorCentroDeTrabajo = new List<PermisoEconomicoPorCentroDeTrabajo>()
            };

            var ctGroup = pe.GroupBy(x => x.TurnoCentroTrabajoId);
            foreach (var g in ctGroup)
            {
                response.PermisoEconomicoPorCentroDeTrabajo.Add(new PermisoEconomicoPorCentroDeTrabajo
                {
                    CentroDeTrabajo = _context.TurnosxCentrosDeTrabajos
                        .Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                        {
                            t.TurnoxCentroDeTrabajoId,
                            t.Turno,
                            c.Nombre,
                            c.Clave
                        })
                        .Where(x => g.Key.HasValue && x.TurnoxCentroDeTrabajoId == g.Key.Value)
                        .Select(s =>
                            (s.Clave.Substring(0, 2) == "03")
                            ? (s.Nombre + ", " + (s.Turno == 0 ? "TM" : "TV"))
                            : (s.Nombre + ", " + (s.Turno == 0 ? "TM" : "TV"))
                        )
                        .FirstOrDefault(),

                    PermisoEconomicos = g.Select(s => new Application.Dtos.Permisos.PermisoEconomico
                    {
                        Id = s.Id,
                        EmpleadoId = s.EmpleadoId,
                        PermisoLaboralId = s.PermisoLaboralId,
                        CentroDeTrabajoId = s.CentroDeTrabajoId,
                        TurnoCentroTrabajoId = s.TurnoCentroTrabajoId,
                        FechaRegistro = s.FechaRegistro,
                        FechaSolicitud = s.FechaSolicitud,
                        LapsoPermisoDiasHabiles = s.LapsoPermisoDiasHabiles,
                        Comentario = s.Comentario ?? "",
                        ComentarioDias = s.ComentarioDias ?? "",
                        ConGoceSueldo = s.ConGoceSueldo,
                        Estatus = s.EstatusPermiso,
                        NombreFirmaAutoriza = _context.Empleados
                            .Where(x => x.EmpleadoId == s.EstatusFirma)
                            .Select(e => $"{e.Nombres} {e.PrimerApellido} {e.SegundoApellido}")
                            .FirstOrDefault(),
                        MotivoRechazo = s.MotivoRechazo ?? ""
                    })
                    .ToList()
                });
            }
            
            return response;
        }
    }
}
