using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class FacuentasBancaria
{
    public int IdCuentasBancarias { get; set; }

    public string EmpleadoId { get; set; } = null!;

    public string? Banco { get; set; }

    public string? NumCuenta { get; set; }

    public string? Clabe { get; set; }

    public string? NumTarjeta { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
