using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CorteTiempo
{
    public int Id { get; set; }

    public string? EmpleadoId { get; set; }

    public int? PermisoLaboralId { get; set; }

    public int? CentroDeTrabajoId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string? Comentario { get; set; }

    public DateTime? HoraSalida { get; set; }

    public int? TiempoEstimado { get; set; }

    public bool? Comprobo { get; set; }

    public int? TurnoCentroTrabajoId { get; set; }

    public DateTime? FechaRegisto { get; set; }

    public int? TiempoReal { get; set; }

    public int? Estatus { get; set; }

    public int? EstatusPermiso { get; set; }

    public string? EstatusFirma { get; set; }

    public string? MotivoRechazo { get; set; }

    public string? MotivoEliminacion { get; set; }
}
