namespace cobach_api.Application.Dtos.Empleado
{
    public class InformacionGeneralResponse
    {
        public string NumeroEmpleado { get; set; } = null!;
        public string CURP { get; set; } = null!;
        public string RFC { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string Nacionalidad { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string EntidadNacimiento { get; set; } = null!;
        public string EstadoCivil { get; set; } = null!;
        public string TipoSangre { get; set; } = null!;
        public string Sexo { get; set; } = null!;
        public string Padecimiento { get; set; } = null!;
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? NSS { get; set; }
        public DateTime? FechaInscripcionIssste { get; set; }
        public bool? AhorroVoluntario { get; set; }
        public Domicilio? Direccion { get; set; }
        public InformacionAcademica? Profesion { get; set; }
        public List<InformacionLaboral>? Laboral { get; set; }

        public class Domicilio
        {
            public string? Calle { get; set; }
            public string? NumeroExterior { get; set; }
            public string? NumeroInterior { get; set; }
            public string? Colonia { get; set; }
            public string? CodigoPostal { get; set; }
            public string? Localidad { get; set; }
        }

        public class InformacionAcademica
        {
            public string? NivelEstudio { get; set; }
            public bool Titulado { get; set; }
            public string? Profesion { get; set; }
            public string? Posgrado { get; set; }
            public string? AreaDesempeño { get; set; }
        }

        public class InformacionLaboral
        {
            public string Proyecto { get; set; } = null!;
            public bool Activo { get; set; }
            public string TipoEmpleado { get; set; } = null!;
            public DateTime FechaIngreso { get; set; }
            public bool Sindicalizado { get; set; }
            public string Plaza { get; set; } = null!;
            public string ConCaracter { get; set; } = null!;
            public string? DenominacionPlaza { get; set; }
            public CentroTrabajo? CentroTrabajo { get; set; }
        }

        public class CentroTrabajo
        {
            public string Clave { get; set; } = null!;
            public string Turno { get; set; } = null!;
            public string Name { get; set; } = null!;
        }
    }
}
