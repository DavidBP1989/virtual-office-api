namespace cobach_api.Application.Dtos.RevisionPermisos
{
    public class PermisosLaborales
    {
        public List<PermisoLaboral>? CortesTiempo { get; set; }
        public List<PermisoLaboral>? PermisosEconomicos { get; set; }
    }

    public class PermisoLaboral
    {
        public int PermisoId { get; set; }
        public string Empleado { get; set; } = null!;
        public string? CentroDeTrabajo { get; set; }
        public DateTime FechaRegisto { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime HoraSalida { get; set; }
        public int TiempoEstimado { get; set; }
        public string? Comentario { get; set; }
        public int Estatus { get; set; }
        public int? TiempoReal { get; set; }
        public int TiempoLimite { get; set; }
        public string ComentarioDias { get; set; } = null!;
        public bool? ConGoceSueldo { get; set; }
        public int? LapsoPermisoDiasHabiles { get; set; }
        public int? DiasReales { get; set; }
    }
}
