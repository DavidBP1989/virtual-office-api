using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class PermisoEconomico
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
