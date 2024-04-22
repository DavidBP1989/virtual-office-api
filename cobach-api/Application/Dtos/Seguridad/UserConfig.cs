namespace cobach_api.Application.Dtos.Seguridad
{
    public class UserConfig
    {
        public string UserId { get; set; } = null!;
        public bool FirstTimeLogin { get; set; }
    }
}
