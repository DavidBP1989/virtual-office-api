using AutoMapper;
using cobach_api.Application.Dtos.Empleado;
using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cobach_api.Features.Empleado
{
    public class CentrosTrabajo
    {
        public record Request : IRequest<ApiResponse<List<Response>>>;
        public class Response : CentrosTrabajoResponse { }
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
                var res = await _context.InformacionLaborals
                    .Join(_context.CentrosDeTrabajos,
                        t => t.TurnoxCentroDeTrabajo.CentroDeTrabajoId,
                        c => c.CentroDeTrabajoId,
                        (t, c) => new
                        {
                            t.TurnoxCentroDeTrabajoId,
                            t.EmpleadoId,
                            t.Status,
                            t.TurnoxCentroDeTrabajo.Turno,
                            c.CentroDeTrabajoId,
                            c.Clave,
                            c.Nombre,
                            c.DomicilioLocalidad,
                            t.StatusBorrado
                        }).Where(x => x.EmpleadoId.Equals(_user.GetCurrentUser()) && x.Status == 1 && x.StatusBorrado == 0)
                        .Select(s => new Response
                        {
                            CentroDeTrabajoId = s.CentroDeTrabajoId,
                            TurnoxCentroDeTrabajoId = s.TurnoxCentroDeTrabajoId,
                            Clave = s.Clave,
                            Nombre = (s.Clave.Substring(0, 2) == "03") ? (s.Nombre.Substring(45, 10) + ", " + (s.Turno == 0 ? "TM" : "TV")) : (s.Nombre + ", " + (s.Turno == 0 ? "TM" : "TV")),
                            Localidad = s.DomicilioLocalidad
                        }).ToListAsync(cancellationToken: cancellationToken);

                return new ApiResponse<List<Response>>(res);
            }
        }
    }
}
