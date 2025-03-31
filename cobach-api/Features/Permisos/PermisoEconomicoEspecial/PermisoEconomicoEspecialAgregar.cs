using cobach_api.Features.Common.Enums;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos.PermisoEconomicoEspecial
{
    public class PermisoEconomicoEspecialAgregar
    {
        public record Request(int CentroDeTrabajoId, string Comentario, string ComentarioDias, DateTime FechaSolicitud, int LapsoDias, string EmpleadoId, bool ConGoce, int? TurnoCentroTrabajoId = null)
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
                    EmpleadoId = request.EmpleadoId,
                    PermisoLaboralId = (int)TipoPermisosLaborales.PermisoEconomico,
                    CentroDeTrabajoId = request.CentroDeTrabajoId,
                    TurnoCentroTrabajoId = request.TurnoCentroTrabajoId,
                    FechaRegistro = DateTime.Now,
                    FechaSolicitud = request.FechaSolicitud,
                    LapsoPermisoDiasHabiles = request.LapsoDias,
                    Comentario = request.Comentario,
                    ComentarioDias = request.ComentarioDias,
                    ConGoceSueldo = request.ConGoce,
                    EstatusFirma = _user.GetCurrentUser(),
                    EstatusPermiso = 6
                };

                _context.PermisoEconomicos.Add(permiso);
                await _context.SaveChangesAsync(cancellationToken);

                return new ApiResponse<Response>(new Response(permiso.Id));
            }
        }
    }
}
