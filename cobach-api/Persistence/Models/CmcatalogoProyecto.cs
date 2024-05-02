using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CmcatalogoProyecto
{
    public int IdCatalogoProyecto { get; set; }

    public string ClaveProyecto { get; set; } = null!;

    public string DescripcionProyecto { get; set; } = null!;

    public string Responsable { get; set; } = null!;

    public bool? Activo { get; set; }
}
