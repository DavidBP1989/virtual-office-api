using cobach_api.Application.Dtos.RevisionPermisos;
using cobach_api.Features.Common.Enums;

namespace cobach_api.Features.RevisionPermisos.Interfaces
{
    public interface IRevisionPermisos
    {
        Task<List<AutorizacionProyectos>> ObtenerAutorizacionProyectos();
        Task<PermisosLaborales> ObtenerPermisosLaboralesPendientesPorRevisar(List<AutorizacionProyectos> autorizacionProyectos);
        Task<bool> ActualizarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId);
        Task<bool> AutorizarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, int tiempoReal, bool justificanteISSSTE);
        Task<bool> RechazarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, string motivo);
        Task<bool> EliminarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId);
        Task<bool> EliminarPermisoLaboral(TipoPermisosLaborales tipoPermiso, int permisoLaboralId, string Motivo);
    }
}
