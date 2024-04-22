using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class TipoSangre
{
    public byte TipoSangreId { get; set; }

    public string? GrupoSanguineo { get; set; }
}
