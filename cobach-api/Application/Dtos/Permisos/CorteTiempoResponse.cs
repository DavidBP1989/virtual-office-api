﻿namespace cobach_api.Application.Dtos.Permisos
{
    public class CortesDeTiempoResponse
    {
        public int? TiempoLimite { get; set; }
        public int? TiempoReal { get; set; }
        public List<CortesDeTiempoPorCentroDeTrabajo> CortesDeTiempoPorCentroDeTrabajo { get; set; } = null!;
    }

    public class CortesDeTiempoPorCentroDeTrabajo
    {
        public string? CentroDeTrabajo { get; set; } = null!;
        public List<CorteDeTiempo> CortesDeTiempo { get; set; } = null!;
    }

    public class CorteDeTiempo
    {
        public int Id { get; set; }
        public string? EmpleadoId { get; set; }
        public int? PermisoLaboralId { get; set; }
        public int? CentroDeTrabajoId { get; set; }
        public string? CentroDeTrabajo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string? Comentario { get; set; }
        public DateTime? HoraSalida { get; set; }
        public int? TiempoEstimado { get; set; }
        public bool? Comprobo { get; set; }
        public int? TurnoCentroTrabajoId { get; set; }
        public DateTime? FechaRegisto { get; set; }
        public int? TiempoReal { get; set; }
        public int? Estatus { get; set; }
        public string? NombreFirmaAutoriza { get; set; }
        public string MotivoRechazo { get; set; } = null!;
    }
}
