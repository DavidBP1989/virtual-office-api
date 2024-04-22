using cobach_api.Application.Dtos.Documentos;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Documentos
{
    public class ArchivosPorFolder
    {
        public record Request(int FolderId) : IRequest<ApiResponse<List<Response>>>;
        public class Response : ArchivosPorFolderResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            private readonly SiiaContext _context;
            private readonly IUserService _user;
            public CommandHandler(SiiaContext context, IUserService user)
            {
                _context = context;
                _user = user;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var files = await _context.Documentos
                    .Where(x => x.TipoDocumentoId == request.FolderId && x.EmpleadoId == _user.GetCurrentUser())
                    .Select(s => new
                    {
                        fileId = s.DocumentoId,
                        description = s.Nombre
                    }).ToListAsync(cancellationToken: cancellationToken);

                var res = new List<Response>();
                foreach (var file in files)
                {
                    res.Add(new Response
                    {
                        ApiRoute = $"/documents/file?fileId={file.fileId}&size=128x128",
                        Description = file.description
                    });
                }

                return new ApiResponse<List<Response>>(res);
            }
        }
    }
}
