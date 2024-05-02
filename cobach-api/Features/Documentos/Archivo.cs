using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Documentos
{
    public class Archivo
    {
        public record Request(string FileId, string Size) : IRequest<ApiResponse<Response>>;
        public record Response(byte[] Image);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly SiiaContext _context;
            readonly IFileService _fileService;
            readonly IUserService _user;
            public CommandHandler(SiiaContext context, IFileService fileService, IUserService user)
            {
                _context = context;
                _fileService = fileService;
                _user = user;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var file = await _context.Documentos
                    .Include(d => d.Empleado)
                    .Where(x => x.DocumentoId == request.FileId)
                    .Select(x => new
                    {
                        name = x.NombreFisico
                    })
                    .SingleAsync(cancellationToken: cancellationToken);

                var image = _fileService.GetImageAsByteArray(_user.GetCurrentUser(), file.name, request.Size);

                return new ApiResponse<Response>(new Response(image));

            }
        }
    }
}
