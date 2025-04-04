﻿using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Infrastructure.Interfaces;
using cobach_api.Persistence;
using static cobach_api.Application.Dtos.Empleado.FondoAhorroResponse;
using static cobach_api.Application.Dtos.Empleado.InformacionGeneralResponse;
using cobach_api.Application.Dtos.Empleado;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Features.Empleado
{
    public class Repository : IEmpleado
    {
        readonly SiiaContext _context;
        readonly IUserService _user;
        public Repository(SiiaContext context, IUserService user)
        {
            _context = context;
            _user = user;
        }

        public async Task<List<int>> ObtenerCentrosDeTrabajo()
        {
            return await _context.InformacionLaborals
                .Join(_context.TurnosxCentrosDeTrabajos,
                    i => i.TurnoxCentroDeTrabajoId,
                    t => t.TurnoxCentroDeTrabajoId, (i, t) => new
                    {
                        i.EmpleadoId,
                        t.CentroDeTrabajoId
                    }
                )
                .Where(x => x.EmpleadoId == _user.GetCurrentUser())
                .Select(x => x.CentroDeTrabajoId)
                .ToListAsync();
        }

        public async Task<FondoAhorroResponse?> ObtenerFondoAhorro()
        {
            int? quincena = _context.Fatransacciones
                    .Where(w => w.IdCatalogoConcepto == 2)
                    .Select(s => s.Quincena)
                    .Max() ?? 0;

            var q = await _context.Faregistros
                .Where(w => w.EmpleadoId == _user.GetCurrentUser() && w.Estado == 1)
                .Select(s => new FondoAhorroResponse
                {
                    IdRegistro = s.IdRegistroFa,
                    FechaIngreso = s.FechaIngreso,
                    Aportacion = s.Aportacion,
                    TipoAportacion = s.TipoAportacion,
                    Quincena = quincena,
                    Acumulado = s.Faacumulados.Where(wa => wa.Quincena == quincena).Select(sa => sa.Monto).FirstOrDefault(),
                    CuentasBancarias = s.Empleado.FacuentasBancaria.Select(sc => new CuentaBancaria
                    {
                        Banco = sc.Banco,
                        NumCuenta = sc.NumCuenta,
                        NumTarjeta = sc.NumTarjeta,
                        Clabe = sc.Clabe
                    }).ToList(),
                    Beneficiarios = s.Fabeneficiarios.Select(sb => new Beneficiario
                    {
                        NombreBeneficiario = sb.NombreBeneficiario,
                        Parentesco = sb.Parentesco,
                        Porcentaje = sb.Porcentaje
                    }).ToList(),
                    Prestamos = s.Faprestamos.Where(wp => wp.Estado == 1).Select(sp => new Prestamo
                    {
                        ImporteSolicitado = sp.ImporteSolicitado,
                        DescuentoQuincenal = sp.DescuentoQuincenal,
                        ResumenSaldo = sp.ResumenSaldo,
                        FechaPrestamo = sp.FechaPrestamo,
                        ImporteTransferencia = sp.ImporteTransferencia,
                        Aval = _context.Empleados.Where(wa => wa.EmpleadoId == sp.Aval).Select(sa => sa.Nombres).FirstOrDefault() + " " + _context.Empleados.Where(wa => wa.EmpleadoId == sp.Aval).Select(sa => sa.PrimerApellido).FirstOrDefault() + " " + _context.Empleados.Where(wa => wa.EmpleadoId == sp.Aval).Select(sa => sa.SegundoApellido).FirstOrDefault()
                    }).ToList()
                }).FirstOrDefaultAsync();

            return q;
        }

        public async Task<List<Application.Dtos.Empleado.FondoAhorroHistorial>> ObtenerFondoAhorroHistorial(int idRegistro)
        {
            return await _context.Fatransacciones
                .Join(_context.Faregistros, ft => ft.IdRegistroFa, fr => fr.IdRegistroFa, (ft, fr) => new { ft, fr })
                .Join(_context.FacatalogoConceptos, all => all.ft.IdCatalogoConcepto, fc => fc.IdCatalogoConcepto, (all, fc) => new { all, fc.Descripcion })
                .Where(x => x.all.ft.IdRegistroFa == idRegistro)
                .Select(s => new Application.Dtos.Empleado.FondoAhorroHistorial
                {
                    Concepto = s.Descripcion ?? "",
                    Quincena = s.all.ft.Quincena.GetValueOrDefault().ToString().Substring(4, 2),
                    Fecha = s.all.ft.FechaTransaccion,
                    Importe = s.all.ft.Importe
                })
                .ToListAsync();
        }

        public async Task<InformacionGeneralResponse> ObtenerInformacionGeneral()
        {
            var q = await _context.Empleados
                .Where(x => x.EmpleadoId == _user.GetCurrentUser())
                .Select(s => new InformacionGeneralResponse
                {
                    NumeroEmpleado = s.NumeroEmpleado ?? "",
                    CURP = s.DocumentacionCurp,
                    RFC = s.DocumentacionRfc,
                    Nombre = s.Nombres,
                    PrimerApellido = s.PrimerApellido,
                    SegundoApellido = s.SegundoApellido,
                    Nacionalidad = _context.Nacionalidads.First(x => x.NacionalidadId == s.Nacionalidad).Nacionalidad1 ?? "",
                    FechaNacimiento = s.FechaNacimiento,
                    EntidadNacimiento = _context.EntidadFederativas.First(x => x.EntidadId == s.DomicilioEntidadFederativa).NombreEntidad ?? "",
                    EstadoCivil = _context.EstadoCivils.First(x => x.EstadoCivilId == s.EstadoCivil).EstadoCivil1 ?? "",
                    TipoSangre = _context.TipoSangres.First(x => x.TipoSangreId == s.TipoSangre).GrupoSanguineo ?? "",
                    Sexo = s.Sexo == 1 ? "Hombre" : "Mujer",
                    Padecimiento = s.Padecimiento ?? "",
                    CorreoElectronico = s.ContactoCorreoElectronico,
                    Telefono = s.ContactoTelefonoLocal,
                    Celular = s.ContactoTelefonoCelular,
                    NSS = s.DocumentacionNss,
                    FechaInscripcionIssste = s.DocumentacionFechaAltaSs,
                    AhorroVoluntario = s.AhorroVoluntario,
                    Profesion = new InformacionAcademica
                    {
                        Titulado = Convert.ToBoolean(s.EscolaridadTitulado),
                        NivelEstudio = _context.NivelEstudios.First(x => x.EscolaridadId == s.EscolaridadNivelEstudios).Escolaridad,
                        Profesion = s.EscolaridadProfesion,
                        Posgrado = s.EscolaridadPosgrado,
                        AreaDesempeño = _context.CamposDisciplinares.First(x => x.CampoDisciplinarId == s.CampoDisciplinarId).Nombre
                    },
                    Direccion = new Domicilio
                    {
                        Calle = s.DomicilioCalle,
                        NumeroExterior = s.DomicilioNumeroExterior,
                        NumeroInterior = s.DomicilioNumeroInterior,
                        CodigoPostal = s.DomicilioCodigoPostal,
                        Colonia = s.DomicilioColonia,
                        Localidad = _context.Localidads.First(x => x.LocalidadId == s.DomicilioLocalidad).Localidad1
                    }
                })
                .FirstAsync();

            q.Laboral = await _context.InformacionLaborals
                .Where(x => x.EmpleadoId == _user.GetCurrentUser() && x.StatusBorrado == 0)
                .Select(s => new InformacionLaboral
                {
                    CentroTrabajo = _context.TurnosxCentrosDeTrabajos
                    .Join(
                        _context.CentrosDeTrabajos,
                        t => t.CentroDeTrabajoId,
                        c => c.CentroDeTrabajoId,
                        (t, c) => new
                        {
                            t.TurnoxCentroDeTrabajoId,
                            t.Turno,
                            c.Clave,
                            c.Nombre,
                        })
                    .Where(x => x.TurnoxCentroDeTrabajoId == s.TurnoxCentroDeTrabajoId)
                    .Select(x => new CentroTrabajo
                    {
                        Clave = x.Clave,
                        Turno = x.Turno == 0 ? "M" : "V",
                        Name = x.Nombre
                    }).First(),
                    Proyecto = _context.CmcatalogoProyectos.First(x => x.IdCatalogoProyecto == s.IdCatalogoProyecto).DescripcionProyecto,
                    Activo = Convert.ToBoolean(s.Status),
                    TipoEmpleado = s.TipoEmpleado == 0
                        ? "Administrativo" : s.TipoEmpleado == 1
                        ? "Docente" : s.TipoEmpleado == 2
                        ? "Directivo" : "",
                    FechaIngreso = s.FechaIngreso,
                    Sindicalizado = Convert.ToBoolean(s.Sindicalizado),
                    IdPlaza = s.IdPlaza,
                    ConCaracter = s.Caracter == 0
                        ? "Base" : s.Caracter == 1
                        ? "Confianza" : s.Caracter == 2
                        ? "Interino" : "",
                    DenominacionPlaza = s.DenominacionPlaza
                })
                .ToListAsync();

            foreach(var l in q.Laboral)
            {
                l.Plaza = l.TipoEmpleado == "Docente"
                    ? _context.CatalogoPlazasDocentes.First(x => x.CatalogoPlazasDocentesId == l.IdPlaza).DescripcionPlazaDocente
                    : _context.CatalogoPlazasAdministrativas.First(x => x.CatalogoPlazasAdministrativasId == l.IdPlaza).DescripcionPlazaAdministrativa;
            }

            return q;
        }
    }
}
