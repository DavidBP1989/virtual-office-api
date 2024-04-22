using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class EntidadFederativa
{
    public byte EntidadId { get; set; }

    public string? NombreEntidad { get; set; }

    public string? ClaveEntidad { get; set; }
}
