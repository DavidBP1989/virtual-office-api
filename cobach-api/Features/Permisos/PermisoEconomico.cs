using cobach_api.Application.Dtos.Permisos;
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
                    .Where(x => x.EmpleadoId.Equals(_user.GetCurrentUser()) &&
                    (x.FechaRegistro.Value.Year == request.Periodo || x.FechaSolicitudInicio.Value.Year == request.Periodo)
                    && x.CentroDeTrabajoId == request.CentroTrabajoId &&
                    (x.TurnoCentroTrabajoId == request.TurnoCentroTrabajoId || request.TurnoCentroTrabajoId == null))
                    .ToListAsync(cancellationToken: cancellationToken);

                var res = new Response
                {
                    //Tomamos los dias permitidos
                    PermisosLimite = _context.CatalogoPermisosLaborales.Where(w => w.Id == 2 && w.Activo.HasValue && w.Activo.Value).FirstOrDefault().TiempoPermitido,
                    PermisosAceptados = permisos.Select(s => s.LapsoPermisoDiasHabiles).DefaultIfEmpty(0).Sum(),
                    Permisos = permisos.Select(p => new PermisoEconomicoList
                    {
                        Id = p.Id,
                        EmpleadoId = p.EmpleadoId,
                        PermisoLaboralId = p.PermisoLaboralId,
                        CentroDeTrabajoId = p.CentroDeTrabajoId,
                        TurnoCentroTrabajoId = p.TurnoCentroTrabajoId,
                        FechaRegistro = p.FechaRegistro,
                        FechaSolicitudInicio = p.FechaSolicitudInicio,
                        FechaSolicitudFinal = p.FechaSolicitudFinal,
                        LapsoPermisoDiasHabiles = p.LapsoPermisoDiasHabiles,
                        Comentario = p.Comentario,
                        ConGoceSueldo = p.ConGoceSueldo
                    }).ToList()
                };

                throw new NotImplementedException();
            }
        }
    }
}
