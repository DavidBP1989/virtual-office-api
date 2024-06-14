using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Common.Enums;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class PermisoEconomico
    {
        public record Request(int Periodo, int CentroTrabajoId, int? TurnoCentroTrabajoId = null) : IRequest<ApiResponse<Response>>;
        public class Response : PermisoEconomicoResponse { }
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
                var permisos = await _context.PermisoEconomicos
                    .Where(x => 
                        x.EmpleadoId == _user.GetCurrentUser() &&
                        x.FechaRegistro.HasValue && x.FechaSolicitud.HasValue &&
                        (x.FechaRegistro.Value.Year == request.Periodo || x.FechaSolicitud.Value.Year == request.Periodo) &&
                        x.CentroDeTrabajoId == request.CentroTrabajoId &&
                        (x.TurnoCentroTrabajoId == request.TurnoCentroTrabajoId || request.TurnoCentroTrabajoId == null)
                    ).ToListAsync(cancellationToken);

                var res = new Response
                {
                    PermisosLimite = _context.CatalogoPermisosLaborales
                        .Where(x => x.Id == (int)TipoPermisosLaborales.PermisoEconomico && x.Activo.HasValue && x.Activo.Value)
                        .First().TiempoPermitido,
                    PermisosAceptados = permisos
                        .Where(x => x.EstatusPermiso == (int)EstatusPermisos.Confirmado)
                        .Select(x => x.LapsoPermisoDiasHabiles)
                        .DefaultIfEmpty(0)
                        .Sum(),
                    Permisos = permisos.Select(p => new PermisoEconomicoList
                    {
                        Id = p.Id,
                        EmpleadoId = p.EmpleadoId,
                        PermisoLaboralId = p.PermisoLaboralId,
                        CentroDeTrabajoId = p.CentroDeTrabajoId,
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
                                }).Where(x => x.TurnoxCentroDeTrabajoId == p.TurnoCentroTrabajoId)
                                .Select(ts => (ts.Clave.Substring(0, 2) == "03") ? (ts.Nombre.Substring(45, 10) + ", " + (ts.Turno == 0 ? "TM" : "TV")) : (ts.Nombre + ", " + (ts.Turno == 0 ? "TM" : "TV"))).FirstOrDefault(),
                        TurnoCentroTrabajoId = p.TurnoCentroTrabajoId,
                        FechaRegistro = p.FechaRegistro,
                        FechaSolicitud = p.FechaSolicitud,
                        LapsoPermisoDiasHabiles = p.LapsoPermisoDiasHabiles,
                        Comentario = p.Comentario ?? "",
                        ComentarioDias = p.ComentarioDias ?? "",
                        ConGoceSueldo = p.ConGoceSueldo,
                        Estatus = p.EstatusPermiso,
                        NombreFirmaAutoriza =
                            _context.Empleados
                            .Where(x => x.EmpleadoId == p.EstatusFirma)
                            .Select(e => $"{e.Nombres} {e.PrimerApellido} {e.SegundoApellido}")
                            .FirstOrDefault(),
                        MotivoRechazo = p.MotivoRechazo ?? ""
                    }).ToList()
                };

                return new ApiResponse<Response>(res);
            }
        }
    }
}
