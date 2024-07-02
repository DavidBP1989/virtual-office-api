using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class FacatalogoConcepto
{
    public int IdCatalogoConcepto { get; set; }

    public string? Descripcion { get; set; }

    public string? Nomenclatura { get; set; }

    /// <summary>
    /// 0 - Egreso
    /// 1 - Ingreso
    /// </summary>
    public byte? TipoConcepto { get; set; }

    public virtual ICollection<Fatransaccione> Fatransacciones { get; set; } = new List<Fatransaccione>();
}
