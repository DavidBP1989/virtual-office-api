using AutoMapper;
using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Features.Permisos.CorteTiempo;
using cobach_api.Features.Permisos.Interfaces;
using cobach_api.Features.Permisos.PermisoEconomico;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos
{
    public class PermisoPorEmpleado
    {
        public record Request(string EmpleadoId) : IRequest<ApiResponse<Response>>;
        public class Response : PermisosPorEmpleado { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            private readonly ICorteTiempo _repositoryCortesTiempo;
            private readonly IPermisoEconomico _repositoryPermisoEconomico;
            private readonly IMapper _mapper;
            private readonly IEmpleado _empleado;
            private readonly IPermisos _permisos;
            public CommandHandler(ICorteTiempo repositoryCortesTiempo, IPermisoEconomico repositoryPermisoEconomico, IMapper mapper, IEmpleado empleado, IPermisos permisos)
            {
                _repositoryCortesTiempo = repositoryCortesTiempo;
                _repositoryPermisoEconomico = repositoryPermisoEconomico;
                _mapper = mapper;
                _empleado = empleado;
                _permisos = permisos;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var fullAccess = await _permisos.PermiteAutorizarTodo();
                List<int>? centrosTrabajo = fullAccess ? null : await _empleado.ObtenerCentrosDeTrabajo();
                var ct = await _repositoryCortesTiempo.GetCortesDeTiempoAsync(request.EmpleadoId, cancellationToken, centrosTrabajo) ?? new CortesDeTiempoResponse();
                var pe = await _repositoryPermisoEconomico.GetPermisoEconomicoAsync(request.EmpleadoId, cancellationToken, centrosTrabajo) ?? new PermisoEconomicoResponse();

                var result = new PermisosPorEmpleado(ct, pe);
                var res = _mapper.Map<Response>(result);
                return new ApiResponse<Response>(res);
            }
        }
    }
}
