using cobach_api.Infrastructure.Interfaces;

namespace cobach_api.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _http;
        public UserService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string GetCurrentUser()
        {
            return _http.HttpContext?.User.Claims.First(x => x.Type == "UserId").Value ?? "";
        }
    }
}