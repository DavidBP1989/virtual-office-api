using cobach_api.Application.Dtos.Empleado;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Empleado
{
    public class InformacionBasica
    {
        public record Request : IRequest<ApiResponse<Response>>;
        public class Response : InformacionBasicaResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly SiiaContext _context;
            readonly IUserService _user;
            readonly IFileService _fileService;
            public CommandHandler(SiiaContext context, IUserService user, IFileService fileService)
            {
                _context = context;
                _user = user;
                _fileService = fileService;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var res = await _context.Empleados
                    .Where(x => x.EmpleadoId.Equals(_user.GetCurrentUser()))
                    .Select(x => new Response
                    {
                        EmpleadoId = x.EmpleadoId,
                        Nombre = $"{x.Nombres} {x.PrimerApellido}"
                    })
                    .FirstAsync(cancellationToken: cancellationToken);

                var doc = await _context.Documentos.FirstOrDefaultAsync(x => x.EmpleadoId == _user.GetCurrentUser() && x.TipoDocumentoId == 5, cancellationToken: cancellationToken);
                if (doc != null)
                {
                    try
                    {
                        
                        res.ProfilePhoto = _fileService.GetImageAsByteArray(_user.GetCurrentUser(), doc.NombreFisico, "");
                    }
                    catch { res.ProfilePhoto = null; }
                }

                return new ApiResponse<Response>(res);
            }
        }
    }
}
