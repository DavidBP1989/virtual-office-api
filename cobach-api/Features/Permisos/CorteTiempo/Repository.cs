using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos.CorteTiempo
{
    public class Repository : ICorteTiempo
    {
        readonly SiiaContext _context;
        public Repository(SiiaContext context)
        {
            _context = context;
        }

        public async Task<CortesDeTiempoResponse> GetCortesDeTiempoAsync(string empleadoId, CancellationToken cancellationToken, List<int>? centrosTrabajo = null)
        {
            int year = DateTime.Now.Year;
            var ct = await _context.CorteTiempos
                .Where(x =>
                    x.EmpleadoId == empleadoId &&
                    x.FechaRegisto.HasValue && x.FechaSolicitud.HasValue &&
                    (
                        x.FechaRegisto.Value.Year == year || x.FechaSolicitud.Value.Year == year
                    ) &&
                    x.EstatusPermiso != (int)EstatusPermisos.Eliminado &&
                    (centrosTrabajo == null || (x.CentroDeTrabajoId.HasValue && centrosTrabajo.Contains(x.CentroDeTrabajoId.Value)))
                )
                .ToListAsync(cancellationToken);

            var response = new CortesDeTiempoResponse
            {
                TiempoLimite = _context.CatalogoPermisosLaborales
                    .First(x => x.Id == (int)TipoPermisosLaborales.CorteTiempo && x.Activo.HasValue && x.Activo.Value)
                    ?.TiempoPermitido,
                TiempoReal = ct
                    .Where(x => x.Comprobo.HasValue && !x.Comprobo.Value)
                    .Select(s => s.TiempoReal)
                    .DefaultIfEmpty(0)
                    .Sum(),
                CortesDeTiempoPorCentroDeTrabajo = new List<CortesDeTiempoPorCentroDeTrabajo>()
            };

            var ctGroup = ct.GroupBy(x => x.TurnoCentroTrabajoId);
            foreach (var g in ctGroup)
            {
                response.CortesDeTiempoPorCentroDeTrabajo.Add(new CortesDeTiempoPorCentroDeTrabajo
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

                    CortesDeTiempo = g.Select(s => new CorteDeTiempo
                    {
                        Id = s.Id,
                        EmpleadoId = s.EmpleadoId,
                        PermisoLaboralId = s.PermisoLaboralId,
                        CentroDeTrabajoId = s.CentroDeTrabajoId,
                        FechaSolicitud = s.FechaSolicitud,
                        Comentario = s.Comentario,
                        HoraSalida = s.HoraSalida,
                        TiempoEstimado = s.TiempoEstimado,
                        Comprobo = s.Comprobo,
                        TurnoCentroTrabajoId = s.TurnoCentroTrabajoId,
                        FechaRegisto = s.FechaRegisto,
                        TiempoReal = s.TiempoReal,
                        Estatus = s.EstatusPermiso,
                        NombreFirmaAutoriza = _context.Empleados
                            .Where(x => x.EmpleadoId == s.EstatusFirma)
                            .Select(x => $"{x.Nombres} {x.PrimerApellido} {x.SegundoApellido}")
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
