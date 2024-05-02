using cobach_api.Application.Dtos.Permisos;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class CorteTiempo
    {
        public record Request : IRequest<ApiResponse<List<Response>>>;
        public class Response : CorteTiempoResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            readonly SiiaContext _context;
            readonly IUserService _user;
            public CommandHandler(SiiaContext context, IUserService user)
            {
                _context = context;
                _user = user;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var res = await _context.CorteTiempos
                    .Where(x => x.EmpleadoId.Equals(_user.GetCurrentUser()))
                    .Select(x => new Response
                    {
                        Id = x.Id,
                        PermisoLaboralId = x.PermisoLaboralId
                    })
                    .ToListAsync(cancellationToken: cancellationToken);

                return new ApiResponse<List<Response>>(res);
            }
        }
    }
}
