using cobach_api.Exceptions;
using cobach_api.Features.Seguridad.Interfaces;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Seguridad
{
    public class CreateToken
    {
        public record Request(string User, string Password) : IRequest<ApiResponse<Response>>;
        public record Response(string Token, bool FirstTimeLogin, bool AllowConfirmWorkPermits);
        
        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IJwtProvider _jwtProvider;
            readonly IAuthentication _authentication;
            public CommandHandler(IJwtProvider jwtProvider, IAuthentication authentication)
            {
                _jwtProvider = jwtProvider;
                _authentication = authentication;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                bool loggedIn = await _authentication.Login(request.User, request.Password);
                if (!loggedIn) throw new ApiException("credenciales incorrectas");

                var config = await _authentication.GetUserConfig(request.User);
                string token = _jwtProvider.Generate((config.UserId, request.User));

                return new ApiResponse<Response>(new Response(token, config.FirstTimeLogin, config.AllowConfirmWorkPermits));
            }
        }
    }
}
