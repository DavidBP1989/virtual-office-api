using cobach_api.Application.Dtos.Documentos;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Documentos
{
    public class InformacionCarpetas
    {
        public record Request : IRequest<ApiResponse<List<Response>>>;
        public class Response : InformacionCarpetasResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            readonly SiiaContext _context;
            readonly IUserService _user;

            public CommandHandler(SiiaContext context, IUserService user)
            {
                _context = context;
                _user = user;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var docs = await _context.TiposDocumentos.Select(s => new Response
                {
                    FolderId = s.TipoDocumentoId,
                    FolderName = s.Descripcion,
                    FilesPerFolder = s.Documentos.Where(w => w.EmpleadoId == _user.GetCurrentUser()).Count()
                }).ToListAsync();

                return new ApiResponse<List<Response>>(docs);
            }
        }
    }
}
