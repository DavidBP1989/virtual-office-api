namespace cobach_api.Infrastructure.Interfaces
{
    public interface IFileService
    {
        byte[] GetImageAsByteArray(string userId, string fileName, string size);
    }
}
