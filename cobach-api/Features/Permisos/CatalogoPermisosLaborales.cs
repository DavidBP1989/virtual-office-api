using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class CatalogoPermisosLaborales
    {
        public record Request : IRequest<ApiResponse<List<Response>>>;
        public record Response(int Id, string PermisoLaboral);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            readonly SiiaContext _context;
            public CommandHandler(SiiaContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var permisos = await _context.CatalogoPermisosLaborales
                    .Where(x => x.Activo.HasValue && x.Activo.Value)
                    .Select(
                        x => new Response(x.Id, x.Nombre ?? "")
                    )
                    .ToListAsync(cancellationToken: cancellationToken);
                return new ApiResponse<List<Response>>(permisos);
            }
        }
    }
}
