using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cobach_api.Features.Empleado
{
    public class Empleados
    {
        public record Request() : IRequest<ApiResponse<List<Response>>>;
        public record Response(string EmpleadoId, string Nombre);
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
                var idProyectos = await _context.InformacionLaborals
                    .Where(x => x.EmpleadoId == _user.GetCurrentUser())
                    .Select(x => x.IdCatalogoProyecto)
                    .ToListAsync(cancellationToken: cancellationToken);
                bool all = await _context.AutorizacionSolicitudes.AnyAsync(x => x.AutorizaPermiso == _user.GetCurrentUser(), cancellationToken: cancellationToken);

                var empleados = await _context.Empleados
                    .Where(x => x.InformacionLaborals.Where(w => idProyectos.Contains(w.IdCatalogoProyecto)).Any() || all)
                    .GroupBy(g => new
                    {
                        g.EmpleadoId,
                        g.Nombres,
                        g.PrimerApellido,
                        g.SegundoApellido
                    })
                    .Select(x =>
                        new Response(x.Key.EmpleadoId, x.Key.Nombres + " " + x.Key.PrimerApellido + " " + x.Key.SegundoApellido)
                    )
                    .ToListAsync(cancellationToken: cancellationToken);

                return new ApiResponse<List<Response>>(empleados);

            }
        }
    }
}
