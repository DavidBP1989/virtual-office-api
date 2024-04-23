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
                var centrosid = await _context.InformacionLaborals.Where(x => x.EmpleadoId.Equals(_user.GetCurrentUser()) && x.Status == 1)
                    .Select(s => s.TurnoxCentroDeTrabajo.CentroDeTrabajoId)
                    .ToListAsync(cancellationToken: cancellationToken);

                var res = await _context.CentrosDeTrabajos.Where(w => centrosid.Contains(w.CentroDeTrabajoId)).Select(s => new Response
                {
                    CentroDeTrabajoId = s.CentroDeTrabajoId,
                    Clave = s.Clave,
                    Nombre = s.Nombre,
                    Localidad = s.DomicilioLocalidad
                }).ToListAsync(cancellationToken: cancellationToken);


                return new ApiResponse<List<Response>>(res);
            }
        }
    }
}
