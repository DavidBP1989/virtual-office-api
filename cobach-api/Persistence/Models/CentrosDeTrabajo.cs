using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class CentrosDeTrabajo
{
    public int CentroDeTrabajoId { get; set; }

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string DomicilioCalle { get; set; } = null!;

    public string DomicilioNumeroExterior { get; set; } = null!;

    public string? DomicilioNumeroInterior { get; set; }

    public string DomicilioColonia { get; set; } = null!;

    public string? DomicilioCodigoPostal { get; set; }

    public string DomicilioLocalidad { get; set; } = null!;

    public string DomicilioEntidadFederativa { get; set; } = null!;

    public string? ContactoCorreoElectronico { get; set; }

    public string ContactoTelefonoLocal { get; set; } = null!;

    public string? ContactoTelefonoCelular { get; set; }

    public string NombreDirector { get; set; } = null!;

    public string NombreAdministrador { get; set; } = null!;
}
