using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class TurnosxCentrosDeTrabajo
{
    public int TurnoxCentroDeTrabajoId { get; set; }

    public string NombreSubdirector { get; set; } = null!;

    public int CentroDeTrabajoId { get; set; }

    public byte Turno { get; set; }

    public virtual ICollection<InformacionLaboral> InformacionLaborals { get; set; } = new List<InformacionLaboral>();
}
