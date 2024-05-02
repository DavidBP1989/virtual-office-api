using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CatalogoPermisosLaborale
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public bool? Activo { get; set; }

    public int? TiempoPermitido { get; set; }
}
