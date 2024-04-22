using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Documento
{
    public string DocumentoId { get; set; } = null!;

    public string EmpleadoId { get; set; } = null!;

    public int TipoDocumentoId { get; set; }

    public string NombreFisico { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public long Bytes { get; set; }

    public DateTime FechaDocumento { get; set; }

    public string? Metadatos { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual TiposDocumento TipoDocumento { get; set; } = null!;
}
