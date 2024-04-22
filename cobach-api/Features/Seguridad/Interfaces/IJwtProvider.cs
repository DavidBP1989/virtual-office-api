namespace cobach_api.Features.Seguridad.Interfaces
{
    public interface IJwtProvider
    {
        public string Generate((string userId, string userName) userDetails);
    }
}
