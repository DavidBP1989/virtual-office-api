using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Persistence.Models;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class PermisoEconomicoActualizar
    {
        public record Request(int PermisoId, int CentroDeTrabajoId, string Comentario, DateTime FechaSolicitudInicio, DateTime FechaSolicitudFinal, int LapsoDias, bool ConGoce, int? TurnoCentroTrabajoId = null)
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
                Persistence.Models.PermisoEconomico permisoEconomico = await _context.PermisoEconomicos.FindAsync(request.PermisoId);
                if (permisoEconomico is null)
                    return new ApiResponse<Response>("El registro no existe.");

                permisoEconomico.CentroDeTrabajoId = request.CentroDeTrabajoId;
                permisoEconomico.Comentario = request.Comentario;
                permisoEconomico.FechaSolicitudInicio = request.FechaSolicitudInicio;
                permisoEconomico.FechaSolicitudFinal = request.FechaSolicitudFinal;
                permisoEconomico.LapsoPermisoDiasHabiles = request.LapsoDias;
                permisoEconomico.ConGoceSueldo = request.ConGoce;
                permisoEconomico.TurnoCentroTrabajoId = request.TurnoCentroTrabajoId;

                _context.Update(permisoEconomico);
                await _context.SaveChangesAsync();

                return new ApiResponse<Response>(new Response(permisoEconomico.Id));
            }
        }
    }
}
