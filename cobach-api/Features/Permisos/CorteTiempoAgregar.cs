using AutoMapper;
using cobach_api.Application.Dtos.Empleado;
using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Permisos.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Infrastructure.Services;
using cobach_api.Persistence;
using cobach_api.Persistence.Models;
using cobach_api.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Permisos
{
    public class CorteTiempoAgregar
    {
        public record Request(int centroDeTrabajo, string comentario, DateTime fechaSolicitud, DateTime horaSalida,int tiempoEstimado, bool comprobo) : IRequest<ApiResponse<Response>>;
        public record Response(int corteID);

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly SiiaContext _context;
            readonly IUserService _user;
            public CommandHandler(SiiaContext context, IUserService user)
            {
                _context = context;
                _user = user;
            }
            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                cobach_api.Persistence.Models.CorteTiempo corteTiempo = new Persistence.Models.CorteTiempo();
                corteTiempo.EmpleadoId = _user.GetCurrentUser();
                corteTiempo.PermisoLaboralId = 1;//CorteTiempo
                corteTiempo.CentroDeTrabajo = request.centroDeTrabajo;
                corteTiempo.FechaSolicitud = request.fechaSolicitud;
                corteTiempo.TiempoEstimado = request.tiempoEstimado;
                corteTiempo.Comentario = request.comentario;
                corteTiempo.HoraSalida = request.horaSalida;
                corteTiempo.Comprobo = request.comprobo;

                _context.CorteTiempos.Add(corteTiempo);
                await _context.SaveChangesAsync();

                return new ApiResponse<Response>(new Response(corteTiempo.Id));
            }
        }
    }
}
