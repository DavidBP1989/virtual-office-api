using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Fabeneficiario
{
    public int IdBeneficiario { get; set; }

    public int IdRegistroFa { get; set; }

    public string? NombreBeneficiario { get; set; }

    public string? Parentesco { get; set; }

    public byte? Porcentaje { get; set; }

    public virtual Faregistro IdRegistroFaNavigation { get; set; } = null!;
}
