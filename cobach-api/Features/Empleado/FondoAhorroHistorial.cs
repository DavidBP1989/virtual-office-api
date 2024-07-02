using AutoMapper;
using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Empleado
{
    public class FondoAhorroHistorial
    {
        public record Request(int IdRegistro) : IRequest<ApiResponse<List<Response>>>;
        public class Response : Application.Dtos.Empleado.FondoAhorroHistorial { }
        public class CommandHandler : IRequestHandler<Request, ApiResponse<List<Response>>>
        {
            readonly IEmpleado _empleado;
            readonly IMapper _mapper;
            public CommandHandler(IEmpleado empleado, IMapper mapper)
            {
                _empleado = empleado;
                _mapper = mapper;
            }

            public async Task<ApiResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var history = await _empleado.ObtenerFondoAhorroHistorial(request.IdRegistro);
                var res = _mapper.Map<List<Response>>(history);

                return new ApiResponse<List<Response>>(res);
            }
        }
    }
}
