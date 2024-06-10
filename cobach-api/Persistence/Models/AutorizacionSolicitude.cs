using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class AutorizacionSolicitude
{
    public int IdProyecto { get; set; }

    public string Autoriza1 { get; set; } = null!;

    public string Autoriza2 { get; set; } = null!;

    public string? AutorizaPermiso { get; set; }
}
