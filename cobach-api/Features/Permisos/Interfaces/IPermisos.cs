using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos.Interfaces
{
    public interface IPermisos
    {
        Task<CorteTiempoResponse> ObtenerCorteTiempo();
    }
}
