namespace cobach_api.Infrastructure.Interfaces
{
    public interface IQRService
    {
        byte[] GetQRCode(string url);
    }
}
