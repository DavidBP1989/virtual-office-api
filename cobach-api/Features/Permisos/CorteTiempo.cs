using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class CorteTiempo
    {
        public record Request(int Periodo, int CentroTrabajoId, int? TurnoCentroTrabajoId = null) : IRequest<ApiResponse<Response>>;
        public class Response : CorteTiempoResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly SiiaContext _context;
            readonly IUserService _user;
            public CommandHandler(SiiaContext context, IUserService user)
            {
                _context = context;
                _user = user;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var cortes = await _context.CorteTiempos
                    .Where(
                        x => x.EmpleadoId == _user.GetCurrentUser() &&
                        x.FechaRegisto.HasValue && x.FechaSolicitud.HasValue &&
                        (x.FechaRegisto.Value.Year == request.Periodo || x.FechaSolicitud.Value.Year == request.Periodo) &&
                        x.CentroDeTrabajoId == request.CentroTrabajoId &&
                        (x.TurnoCentroTrabajoId == request.TurnoCentroTrabajoId || request.TurnoCentroTrabajoId == null)
                    )
                    .ToListAsync(cancellationToken: cancellationToken);

                var res = new Response
                {
                    TiempoLimite = _context.CatalogoPermisosLaborales
                        .Where(w => w.Id == (int)TipoPermisosLaborales.CorteTiempo && w.Activo.HasValue && w.Activo.Value)
                        .First().TiempoPermitido,
                    TiempoReal = cortes
                        .Select(s => s.TiempoReal)
                        .DefaultIfEmpty(0)
                        .Sum(),
                    Cortes = cortes.Select(c => new CorteTiempoList
                    {
                        Id = c.Id,
                        EmpleadoId = c.EmpleadoId,
                        PermisoLaboralId = c.PermisoLaboralId,
                        CentroDeTrabajoId = c.CentroDeTrabajoId,
                        CentroDeTrabajo = _context.TurnosxCentrosDeTrabajos
                            .Join(_context.CentrosDeTrabajos,
                                t => t.CentroDeTrabajoId,
                                c => c.CentroDeTrabajoId,
                                (t, c) => new
                                {
                                    t.TurnoxCentroDeTrabajoId,
                                    t.Turno,
                                    c.Nombre,
                                    c.Clave
                                }).Where(x => x.TurnoxCentroDeTrabajoId == c.TurnoCentroTrabajoId)
                                .Select(ts => (ts.Clave.Substring(0, 2) == "03") ? (ts.Nombre.Substring(45, 10) + ", " + (ts.Turno == 0 ? "TM" : "TV")) : (ts.Nombre + ", " + (ts.Turno == 0 ? "TM" : "TV"))).FirstOrDefault(),
                        FechaSolicitud = c.FechaSolicitud,
                        Comentario = c.Comentario,
                        HoraSalida = c.HoraSalida,
                        TiempoEstimado = c.TiempoEstimado,
                        Comprobo = c.Comprobo,
                        TurnoCentroTrabajoId = c.TurnoCentroTrabajoId,
                        FechaRegisto = c.FechaRegisto,
                        TiempoReal = c.TiempoReal,
                        Estatus = c.EstatusPermiso,
                        NombreFirmaAutoriza = 
                            _context.Empleados
                            .Where(x => x.EmpleadoId == c.EstatusFirma)
                            .Select(e => $"{e.Nombres} {e.PrimerApellido} {e.SegundoApellido}")
                            .FirstOrDefault(),
                        MotivoRechazo = c.MotivoRechazo ?? ""
                    }).ToList()
                };

                return new ApiResponse<Response>(res);
            }
        }
    }
}
