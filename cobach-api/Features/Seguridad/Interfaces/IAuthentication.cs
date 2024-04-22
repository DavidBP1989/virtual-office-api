using cobach_api.Application.Dtos.Seguridad;

namespace cobach_api.Features.Seguridad.Interfaces
{
    public interface IAuthentication
    {
        Task<bool> Login(string user, string password);
        Task<UserConfig> GetUserConfig(string user);
    }
}
