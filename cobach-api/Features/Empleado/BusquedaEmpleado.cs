using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Empleado
{
    public class BusquedaEmpleado
    {
        public record Request(string Filtro) : IRequest<ApiResponse<List<Response>>>;
        public record Response(string EmpleadoId, string Nombre);
        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            readonly SiiaContext _context;
            public CommandHandler(SiiaContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                string f = request.Filtro.ToLower();
                var res = await _context.Empleados
                    .Where(x => (x.Nombres.ToLower() + " " + x.PrimerApellido.ToLower() + " " + x.SegundoApellido.ToLower()).Contains(f))
                    .Select(x => new Response(x.EmpleadoId, $"{x.Nombres} {x.PrimerApellido} {x.SegundoApellido}".Trim()))
                    .Take(10)
                    .ToListAsync(cancellationToken: cancellationToken);
                return new ApiResponse<List<Response>>(res);
                    
            }
        }
    }
}
