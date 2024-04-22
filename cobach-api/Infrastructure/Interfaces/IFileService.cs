namespace cobach_api.Infrastructure.Interfaces
{
    public interface IFileService
    {
        byte[] GetImageAsByteArray(string fileName, string size);
    }
}
