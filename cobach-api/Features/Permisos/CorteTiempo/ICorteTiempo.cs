using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos.CorteTiempo
{
    public interface ICorteTiempo
    {
        Task<CortesDeTiempoResponse> GetCortesDeTiempoAsync(string empleadoId, CancellationToken cancellationToken, List<int>? centrosTrabajo = null);
    }
}
