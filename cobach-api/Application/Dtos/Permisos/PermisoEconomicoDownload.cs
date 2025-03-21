namespace cobach_api.Application.Dtos.Permisos
{
    public class PermisoEconomicoDownload
    {
        public string DepartamentoAdscripcion { get; set; } = null!;
        public string PuestoEmpleado { get; set; } = null!;
        public string NombreEmpleado { get; set; } = null!;
        public string FechaSolicitud { get; set; } = null!;
        public string FechaInicio { get; set; } = null!;
        public string LugarElaboracion { get; set; } = null!;
        public string LapsoPermiso { get; set; } = null!;
        public string CongoceSueldo { get; set; } = null!;
        public string MotivoSolicitud { get; set; } = null!;
        public string FirmaDigital { get; set; } = null!;
        public string ImagenQR { get; set; } = null!;
    }
}
