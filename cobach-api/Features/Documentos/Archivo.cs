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
        public record Response(byte[]? Image);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            private readonly SiiaContext _context;
            private readonly IFileService _fileService;
            public CommandHandler(SiiaContext context, IFileService fileService)
            {
                _context = context;
                _fileService = fileService;
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

                var image = _fileService.GetImageAsByteArray(file.name, request.Size);

                return new ApiResponse<Response>(new Response(image));

            }
        }
    }
}
