using cobach_api.Features.Common.Enums;
using cobach_api.Features.Permisos.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos
{
    public class Descargar
    {
        public record Request(int PermissionId, TipoPermisosLaborales TipoPermiso) : IRequest<ApiResponse<Response>>;
        public record Response(byte[] File);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IFileService _fileService;
            readonly IPermisos _permisos;
            public CommandHandler(IFileService fileService, IPermisos permisos)
            {
                _fileService = fileService;
                _permisos = permisos;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                Dictionary<string, string>  formFieldMap = request.TipoPermiso switch
                {
                    TipoPermisosLaborales.CorteTiempo => await _permisos.GetCorteTiempoToDownload(request.PermissionId),
                    TipoPermisosLaborales.PermisoEconomico => await _permisos.GetPermisoEconomicoDownload(request.PermissionId),
                    _ => new Dictionary<string, string>()
                };

                var file = _fileService.GetPDFPermission(request.TipoPermiso, formFieldMap);
                return new ApiResponse<Response>(
                    new Response(file)
                );
            }
        }
    }
}
