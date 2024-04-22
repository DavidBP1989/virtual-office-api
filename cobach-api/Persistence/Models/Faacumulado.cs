using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Faacumulado
{
    public int IdFaacumulado { get; set; }

    public int IdRegistroFa { get; set; }

    public int? IdFaregistroDistribucion { get; set; }

    public int Ejercicio { get; set; }

    public int Quincena { get; set; }

    public double Monto { get; set; }

    public double? Factor { get; set; }

    public double? Interes { get; set; }

    public virtual Faregistro IdRegistroFaNavigation { get; set; } = null!;
}
