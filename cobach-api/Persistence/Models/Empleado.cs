using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Empleado
{
    public string EmpleadoId { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string Nombres { get; set; } = null!;

    public byte? EstadoCivil { get; set; }

    public string? Padecimiento { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public byte? Sexo { get; set; }

    public byte? TipoSangre { get; set; }

    public byte? Nacionalidad { get; set; }

    public string DomicilioCalle { get; set; } = null!;

    public string? DomicilioNumeroExterior { get; set; }

    public string? DomicilioNumeroInterior { get; set; }

    public string DomicilioColonia { get; set; } = null!;

    public string? DomicilioCodigoPostal { get; set; }

    public byte? DomicilioLocalidad { get; set; }

    public byte? DomicilioEntidadFederativa { get; set; }

    public string? ContactoCorreoElectronico { get; set; }

    public string? ContactoTelefonoLocal { get; set; }

    public string? ContactoTelefonoCelular { get; set; }

    public string DocumentacionCurp { get; set; } = null!;

    public string DocumentacionRfc { get; set; } = null!;

    public string? DocumentacionNss { get; set; }

    public DateTime? DocumentacionFechaAltaSs { get; set; }

    public byte? EscolaridadNivelEstudios { get; set; }

    public string? EscolaridadProfesion { get; set; }

    public string? EscolaridadPosgrado { get; set; }

    /// <summary>
    /// Campo para indicar si el empleado cuenta con titulo, se manejara con numeros simulando un booleano
    /// </summary>
    public byte? EscolaridadTitulado { get; set; }

    public int CampoDisciplinarId { get; set; }

    public string? NumeroEmpleado { get; set; }

    public bool? AhorroVoluntario { get; set; }

    public byte? PorcentajeAhorro { get; set; }

    public string? Usuario { get; set; }

    public string? ClaveUsuario { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual ICollection<FacuentasBancaria> FacuentasBancaria { get; set; } = new List<FacuentasBancaria>();

    public virtual ICollection<Faprestamo> Faprestamos { get; set; } = new List<Faprestamo>();

    public virtual ICollection<Faregistro> Faregistros { get; set; } = new List<Faregistro>();

    public virtual ICollection<InformacionLaboral> InformacionLaborals { get; set; } = new List<InformacionLaboral>();
}
