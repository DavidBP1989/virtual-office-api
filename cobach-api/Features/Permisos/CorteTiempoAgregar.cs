using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos
{
    public class CorteTiempoAgregar
    {
        public record Request(int CentroDeTrabajoId, string Comentario, DateTime FechaSolicitud, DateTime HoraSalida, int TiempoEstimado, bool Comprobo, int? TurnoCentroTrabajoId = null)
            : IRequest<ApiResponse<Response>>;
        public record Response(int CorteID);

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
                Persistence.Models.CorteTiempo corteTiempo = new()
                {
                    EmpleadoId = _user.GetCurrentUser(),
                    PermisoLaboralId = 1,
                    CentroDeTrabajoId = request.CentroDeTrabajoId,
                    TurnoCentroTrabajoId = request.TurnoCentroTrabajoId,
                    FechaSolicitud = request.FechaSolicitud,
                    TiempoEstimado = request.TiempoEstimado,
                    Comentario = request.Comentario,
                    HoraSalida = request.HoraSalida,
                    Comprobo = request.Comprobo,
                    FechaRegisto = DateTime.Now,
                    Estatus = 0
                };

                _context.CorteTiempos.Add(corteTiempo);
                await _context.SaveChangesAsync(cancellationToken);

                return new ApiResponse<Response>(new Response(corteTiempo.Id));
            }
        }
    }
}
