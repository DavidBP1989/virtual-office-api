namespace cobach_api.Application.Dtos.Permisos
{
    public class PermisoEconomicoResponse
    {
        public int? PermisosLimite { get; set; }
        public int? PermisosAceptados { get; set; }
        public List<PermisoEconomicoList> Permisos { get; set; } = null!;
    }

    public class PermisoEconomicoList
    {
        public int Id { get; set; }
        public string? EmpleadoId { get; set; }
        public int? PermisoLaboralId { get; set; }
        public int? CentroDeTrabajoId { get; set; }
        public int? TurnoCentroTrabajoId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaSolicitudInicio { get; set; }
        public DateTime? FechaSolicitudFinal { get; set; }
        public int? LapsoPermisoDiasHabiles { get; set; }
        public string? Comentario { get; set; }
        public bool? ConGoceSueldo { get; set; }
    }
}
