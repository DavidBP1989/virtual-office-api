namespace cobach_api.Application.Dtos.Permisos
{
    public class PermisoEconomicoResponse
    {
        public int? PermisosLimite { get; set; }
        public int? PermisosAceptados { get; set; }
        public List<PermisoEconomicoPorCentroDeTrabajo> PermisoEconomicoPorCentroDeTrabajo { get; set; } = null!;
    }

    public class PermisoEconomicoPorCentroDeTrabajo
    {
        public string? CentroDeTrabajo { get; set; } = null!;
        public List<PermisoEconomico> PermisoEconomicos { get; set; } = null!;
    }

    public class PermisoEconomico
    {
        public int Id { get; set; }
        public string? EmpleadoId { get; set; }
        public int? PermisoLaboralId { get; set; }
        public int? CentroDeTrabajoId { get; set; }
        public string? CentroDeTrabajo { get; set; }
        public int? TurnoCentroTrabajoId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public int? LapsoPermisoDiasHabiles { get; set; }
        public string Comentario { get; set; } = null!;
        public string ComentarioDias { get; set; } = null!;
        public bool? ConGoceSueldo { get; set; }
        public int? Estatus { get; set; }
        public string? NombreFirmaAutoriza { get; set; }
        public string MotivoRechazo { get; set; } = null!;
    }
}
