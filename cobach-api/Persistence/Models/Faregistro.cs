using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Faregistro
{
    public int IdRegistroFa { get; set; }

    public string EmpleadoId { get; set; } = null!;

    public bool? Socio { get; set; }

    public DateTime FechaIngreso { get; set; }

    public double? Aportacion { get; set; }

    /// <summary>
    /// Fijo o Variable
    /// </summary>
    public bool? TipoAportacion { get; set; }

    public byte Estado { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual ICollection<Faacumulado> Faacumulados { get; set; } = new List<Faacumulado>();

    public virtual ICollection<Fabeneficiario> Fabeneficiarios { get; set; } = new List<Fabeneficiario>();

    public virtual ICollection<Faprestamo> Faprestamos { get; set; } = new List<Faprestamo>();

    public virtual ICollection<Fatransaccione> Fatransacciones { get; set; } = new List<Fatransaccione>();
}
