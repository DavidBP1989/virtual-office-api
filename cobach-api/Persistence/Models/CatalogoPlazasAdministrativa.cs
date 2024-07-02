using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CatalogoPlazasAdministrativa
{
    public byte CatalogoPlazasAdministrativasId { get; set; }

    public string ClavePlazaAdministrativa { get; set; } = null!;

    public string SiglasPlazaAdministrativa { get; set; } = null!;

    public string DescripcionPlazaAdministrativa { get; set; } = null!;

    public string? TipoAdministrativo { get; set; }

    public string NivelAdministrativo { get; set; } = null!;

    public string? CategoriaPlaza { get; set; }

    public double? SueldoMensualPlaza { get; set; }

    public double? SueldoMensPlazaSindicalizada { get; set; }

    public double? SueldoFederal { get; set; }

    public string? Ejercicio { get; set; }
}
