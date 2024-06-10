using cobach_api.Features.Common.Enums;

namespace cobach_api.Application.Dtos.Seguridad
{
    public class UserConfig
    {
        public string UserId { get; set; } = null!;
        public bool FirstTimeLogin { get; set; }
        public bool AllowConfirmWorkPermits { get; set; }
    }
}
