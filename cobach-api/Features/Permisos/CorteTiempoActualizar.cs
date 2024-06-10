using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class CorteTiempoActualizar
    {
        public record Request(int CorteId, int CentroDeTrabajoId, string Comentario, DateTime FechaSolicitud, DateTime HoraSalida, int TiempoEstimado, int? TurnoCentroTrabajoId = null)
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
                Persistence.Models.CorteTiempo corteTiempo = await _context.CorteTiempos.FindAsync(request.CorteId);

                if(corteTiempo is null)
                    return new ApiResponse<Response>("El registro no existe.");

                if(corteTiempo.EstatusPermiso == 2 || corteTiempo.EstatusPermiso == 3)
                    return new ApiResponse<Response>("El registro no se puede actualizar por que su estatus es: " + corteTiempo.EstatusPermiso);

                corteTiempo.CentroDeTrabajoId = request.CentroDeTrabajoId;
                corteTiempo.Comentario = request.Comentario;
                corteTiempo.FechaSolicitud = request.FechaSolicitud;
                corteTiempo.HoraSalida = request.HoraSalida;
                corteTiempo.TiempoEstimado= request.TiempoEstimado;
                corteTiempo.TurnoCentroTrabajoId = request.TurnoCentroTrabajoId;
                corteTiempo.EstatusPermiso = 0;

                _context.Update(corteTiempo);
                await _context.SaveChangesAsync();

                return new ApiResponse<Response>(new Response(corteTiempo.Id));
            }
        }
    }
}
