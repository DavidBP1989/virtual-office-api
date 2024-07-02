namespace cobach_api.Features.Permisos.Interfaces
{
    public interface IPermisos
    {
        Task<Dictionary<string, string>> GetCorteTiempoToDownload(int permissionId);
        Task<Dictionary<string, string>> GetPermisoEconomicoDownload(int permissionId);
    }
}
