using AutoMapper;
using cobach_api.Application.Dtos.Empleado;
using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Empleado
{
    public class InformacionGeneral
    {
        public record Request : IRequest<ApiResponse<Response>>;
        public class Response : InformacionGeneralResponse { }

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            private readonly IEmpleado _empleado;
            private readonly IMapper _mapper;
            public CommandHandler(IEmpleado empleado, IMapper mapper)
            {
                _empleado = empleado;
                _mapper = mapper;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var inf = await _empleado.ObtenerInformacionGeneral();
                var res = _mapper.Map<Response>(inf);

                return new ApiResponse<Response>(res);
            }
        }
    }
}
