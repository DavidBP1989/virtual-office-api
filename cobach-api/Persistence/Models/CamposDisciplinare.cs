using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CamposDisciplinare
{
    public int CampoDisciplinarId { get; set; }

    public string Nombre { get; set; } = null!;
}
