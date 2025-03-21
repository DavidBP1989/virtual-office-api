using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CatalogoPlazasDocente
{
    public byte CatalogoPlazasDocentesId { get; set; }

    public string ClavePlazaDocente { get; set; } = null!;

    public string TipoDocente { get; set; } = null!;

    public string DescripcionPlazaDocente { get; set; } = null!;

    public double CostoHsm { get; set; }

    public double? CostoHsmsindicalizado { get; set; }

    public int HorasFederales { get; set; }

    public double CostoHf { get; set; }

    public string Ejercicio { get; set; } = null!;
}
