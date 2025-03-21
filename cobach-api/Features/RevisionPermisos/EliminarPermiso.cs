using cobach_api.Exceptions;
using cobach_api.Features.Common.Enums;
using cobach_api.Features.RevisionPermisos.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.RevisionPermisos
{
    public class EliminarPermiso
    {
        public record Request(TipoPermisosLaborales TypeWorkPermit, int WorkPermitId, string Motivo) : IRequest<ApiResponse<Response>>;
        public record Response();

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IRevisionPermisos _revisionPermisos;
            public CommandHandler(IRevisionPermisos revisionPermisos)
            {
                _revisionPermisos = revisionPermisos;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                bool status;
                if (string.IsNullOrEmpty(request.Motivo)) status = await _revisionPermisos.EliminarPermisoLaboral(request.TypeWorkPermit, request.WorkPermitId);
                else status = await _revisionPermisos.EliminarPermisoLaboral(request.TypeWorkPermit, request.WorkPermitId, request.Motivo);

                if (!status) throw new ApiException("Error al intentar elminar el estatus");

                return new ApiResponse<Response>(new Response());
            }
        }
    }
}
