namespace cobach_api.Application.Dtos.Seguridad
{
    public class UserConfig
    {
        public string UserId { get; set; } = null!;
        public bool FirstTimeLogin { get; set; }
        public bool AllowConfirmWorkPermits { get; set; }
        public bool ConfirmedAsCampus { get; set; }
        public bool ShowHistory { get; set; }
    }
}
