using cobach_api.Exceptions;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using cobach_api.Wrappers;
using MediatR;

namespace cobach_api.Features.Empleado
{
    public class CambiarContrasena
    {
        public record Request(string NuevaContrasena) : IRequest<ApiResponse<Response>>;
        public record Response();

        public class CommandHandler : IRequestHandler<Request, ApiResponse<Response>>
        {
            readonly IUserService _user;
            readonly SiiaContext _context;
            public CommandHandler(IUserService user, SiiaContext context)
            {
                _user = user;
                _context = context;
            }

            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = _context.Empleados.FirstOrDefault(x => x.EmpleadoId == _user.GetCurrentUser()) ?? throw new ApiException("empleado no encontrado");

                if (user.ClaveUsuario == request.NuevaContrasena) throw new ApiException("la contraseña nueva no puede ser igual que el nombre de usuario");

                user.ClaveUsuario = request.NuevaContrasena;
                int result = await _context.SaveChangesAsync(cancellationToken);
                return new ApiResponse<Response> { Succeeded = result > 0 };
            }
        }
    }
}
