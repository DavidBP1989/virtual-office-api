using AutoMapper;
using cobach_api.Application.Dtos.Permisos;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Permisos.CorteTiempo
{
    public class CorteTiempoList
    {
        public record Request() : IRequest<ApiResponse<Response>>;
        public class Response : CortesDeTiempoResponse { }
        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly ICorteTiempo _repository;
            readonly IMapper _mapper;
            readonly IUserService _user;
            public CommandHandler(ICorteTiempo repository, IMapper mapper, IUserService user)
            {
                _repository = repository;
                _mapper = mapper;
                _user = user;
            }
            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetCortesDeTiempoAsync(_user.GetCurrentUser(), cancellationToken) ?? new CortesDeTiempoResponse();
                var res = _mapper.Map<Response>(result);

                return new ApiResponse<Response>(res);
            }
        }
    }
}
