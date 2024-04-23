namespace cobach_api.Application.Dtos.Permisos
{
    public class CorteTiempoResponse
    {
        public int Id { get; set; }
        public string? EmpleadoId { get; set; }
        public int? PermisoLaboralId { get; set; }
        public int? CentroDeTrabajo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string? Comentario { get; set; }
        public DateTime? HoraSalida { get; set; }
        public int? TiempoEstimado { get; set; }
        public bool? Comprobo { get; set; }
    }
}
