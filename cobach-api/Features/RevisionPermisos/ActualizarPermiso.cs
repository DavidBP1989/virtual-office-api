using cobach_api.Exceptions;
using cobach_api.Features.Common.Enums;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.RevisionPermisos
{
    public class ActualizarPermiso
    {
        public record Request(TipoPermisosLaborales TypeWorkPermit, int WorkPermitId) : IRequest<ApiResponse<Response>>;
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
                bool status = await _revisionPermisos.ActualizarPermisoLaboral(request.TypeWorkPermit, request.WorkPermitId);
                if (!status) throw new ApiException("Error al intentar actualizar el estatus");

                return new ApiResponse<Response>(new Response(request.WorkPermitId));
            }
        }
    }
}
