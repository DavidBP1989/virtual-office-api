using cobach_api.Exceptions;
using cobach_api.Features.Common.Enums;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.RevisionPermisos
{
    public class RechazarPermiso
    {
        public record Request(TipoPermisosLaborales TypeWorkPermit, int WorkPermitId, string Motive) : IRequest<ApiResponse<Response>>;
        public record Response(int WorkPermitId);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IRevisionPermisos _revisionPermisos;
            public CommandHandler(IRevisionPermisos revisionPermisos)
            {
                _revisionPermisos = revisionPermisos;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                bool status = await _revisionPermisos.RechazarPermisoLaboral(request.TypeWorkPermit, request.WorkPermitId, request.Motive);
                if (!status) throw new ApiException("Error al intentar rechazar el permiso");

                return new ApiResponse<Response>(new Response(request.WorkPermitId));
            }
        }
    }
}
