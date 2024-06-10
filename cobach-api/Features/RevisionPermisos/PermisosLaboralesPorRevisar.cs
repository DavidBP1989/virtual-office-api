using cobach_api.Application.Dtos.RevisionPermisos;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.RevisionPermisos
{
    public class PermisosLaboralesPorRevisar
    {
        public record Request : IRequest<ApiResponse<Response>>;
        public record Response(PermisosLaborales PermisosLaborales);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IRevisionPermisos _revisionPermisos;
            public CommandHandler(IRevisionPermisos revisionPermisos)
            {
                _revisionPermisos = revisionPermisos;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                PermisosLaborales permisosLaborales = new();
                var autorizacionPermisos = await _revisionPermisos.ObtenerAutorizacionProyectos();
                if (autorizacionPermisos.Count > 0)
                {
                    permisosLaborales = await _revisionPermisos.ObtenerPermisosLaboralesPendientesPorRevisar(autorizacionPermisos);
                }
                
                return new ApiResponse<Response>(new Response(permisosLaborales));
            }
        }
    }
}
