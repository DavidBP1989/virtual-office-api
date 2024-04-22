using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class TiposDocumento
{
    public int TipoDocumentoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
