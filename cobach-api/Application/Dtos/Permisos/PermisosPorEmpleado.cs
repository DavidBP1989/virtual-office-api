namespace cobach_api.Application.Dtos.Permisos
{
    public class PermisosPorEmpleado
    {
        public CortesDeTiempoResponse CortesTiempo { get; set; } = null!;
        public PermisoEconomicoResponse PermisosEconomicos { get; set; } = null!;
        public PermisosPorEmpleado(CortesDeTiempoResponse ct, PermisoEconomicoResponse pe)
        {
            CortesTiempo = ct;
            PermisosEconomicos = pe;
        }
        public PermisosPorEmpleado()
        {
            
        }
    }
}
