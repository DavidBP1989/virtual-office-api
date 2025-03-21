using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos.PermisoEconomico
{
    public interface IPermisoEconomico
    {
        Task<PermisoEconomicoResponse> GetPermisoEconomicoAsync(string empleadoId, CancellationToken cancellationToken, List<int>? centrosTrabajo = null);
    }
}
