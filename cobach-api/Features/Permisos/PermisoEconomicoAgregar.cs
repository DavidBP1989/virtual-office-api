using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos
{
    public class PermisoEconomicoAgregar
    {
        public record Request(int CentroDeTrabajoId, string Comentario, DateTime FechaSolicitudInicio, DateTime FechaSolicitudFinal, int LapsoDias, bool ConGoce, int? TurnoCentroTrabajoId = null)
            : IRequest<ApiResponse<Response>>;
        public record Response(int PermisoId);

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
                Persistence.Models.PermisoEconomico permiso = new()
                {
                    EmpleadoId = _user.GetCurrentUser(),
                    PermisoLaboralId = 2,
                    CentroDeTrabajoId = request.CentroDeTrabajoId,
                    TurnoCentroTrabajoId = request.TurnoCentroTrabajoId,
                    FechaRegistro = DateTime.Now,
                    FechaSolicitudInicio = request.FechaSolicitudInicio,
                    FechaSolicitudFinal = request.FechaSolicitudFinal,
                    LapsoPermisoDiasHabiles = request.LapsoDias,
                    Comentario = request.Comentario,
                    ConGoceSueldo = request.ConGoce
                };

                _context.PermisoEconomicos.Add(permiso);
                await _context.SaveChangesAsync(cancellationToken);

                return new ApiResponse<Response>(new Response(permiso.Id));
            }
        }
    }
}
