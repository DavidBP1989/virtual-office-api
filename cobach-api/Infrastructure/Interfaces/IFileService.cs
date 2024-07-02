using cobach_api.Features.Common.Enums;

namespace cobach_api.Infrastructure.Interfaces
{
    public interface IFileService
    {
        byte[] GetImageAsByteArray(string userId, string fileName, string size);
        byte[] GetPDFPermission(TipoPermisosLaborales tipoPermiso, Dictionary<string, string> workPermit);
    }
}
