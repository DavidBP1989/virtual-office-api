using cobach_api.Application.Dtos.Permisos;
using cobach_api.Features.Permisos.Interfaces;
using cobach_api.Persistence;
using cobach_api.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace cobach_api.Features.Permisos
{
    public class Repository : IPermisos
    {
        readonly SiiaContext _context;
        public Repository(SiiaContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetCorteTiempoToDownload(int permissionId)
        {
            var formFieldMap = new Dictionary<string, string>();

            var corteTiempo = await _context.CorteTiempos
                .Join(_context.InformacionLaborals, c => c.EmpleadoId, i => i.EmpleadoId, (c, i) => new { c, i.IdCatalogoProyecto })
                .Join(_context.Empleados, cie => cie.c.EmpleadoId, e => e.EmpleadoId, (cie, e) => new
                { 
                    cie,
                    e.Nombres,
                    e.PrimerApellido,
                    e.SegundoApellido
                })
                .Where(x => x.cie.c.Id == permissionId)
                .Select(s => new CorteTiempoDownload
                {
                    DepartamentoAdscripcion = $"{_context.CmcatalogoProyectos.First(x => x.IdCatalogoProyecto == s.cie.IdCatalogoProyecto).DescripcionProyecto} / {_context.TurnosxCentrosDeTrabajos.Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                        {
                            t.TurnoxCentroDeTrabajoId,
                            t.Turno,
                            c.Nombre,
                            c.Clave
                        })
                        .Where(x => x.TurnoxCentroDeTrabajoId == s.cie.c.TurnoCentroTrabajoId)
                        .Select(ts => (ts.Clave.Substring(0, 2) == "03") ? $"{ts.Nombre.Substring(45, 10)}, {(ts.Turno == 0 ? "TM" : "TV")}" : (ts.Nombre + ", " + (ts.Turno == 0 ? "TM" : "TV")))
                        .FirstOrDefault()
                    }",
                    NombreEmpleado = $"{s.Nombres} {s.PrimerApellido} {s.SegundoApellido}",
                    FechaSolicitud = s.cie.c.FechaSolicitud.GetValueOrDefault().ToString("dd MMMM yyyy"),
                    HoraSalida = $"{s.cie.c.HoraSalida.GetValueOrDefault():hh:mm} hrs",
                    TiempoEstimado = $"{s.cie.c.TiempoEstimado.GetValueOrDefault() / 60} {(s.cie.c.TiempoEstimado.GetValueOrDefault() == 60 ? "Hora" : "Horas")}",
                    TiempoReal = $"{s.cie.c.TiempoReal.GetValueOrDefault() / 60} {(s.cie.c.TiempoReal.GetValueOrDefault() == 60 ? "Hora" : "Horas")}",
                    ObjetivoCorteTiempo = s.cie.c.Comentario ?? "",
                }).FirstOrDefaultAsync();

            if (corteTiempo is null)
                return formFieldMap;

            corteTiempo.FirmaDigital = $"==#{GetDigitalSignature($"{corteTiempo.NombreEmpleado}|!{permissionId}/corteTiempo")}";

            formFieldMap = corteTiempo.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => $"{char.ToLower(prop.Name[0])}{prop.Name.Substring(1)}", prop => prop.GetValue(corteTiempo) as string ?? "");

            return formFieldMap;
        }

        public async Task<Dictionary<string, string>> GetPermisoEconomicoDownload(int permissionId)
        {
            var formFieldMap = new Dictionary<string, string>();

            var permisoEconomico = await _context.PermisoEconomicos
                .Join(_context.InformacionLaborals, pe => pe.EmpleadoId, i => i.EmpleadoId, (pe, i) => new { pe, i.IdCatalogoProyecto, i.DenominacionPlaza })
                .Join(_context.Empleados, pee => pee.pe.EmpleadoId, e => e.EmpleadoId, (pee, e) => new
                {
                    pee,
                    e.Nombres,
                    e.PrimerApellido,
                    e.SegundoApellido,
                    e.DomicilioLocalidad
                })
                .Where(x => x.pee.pe.Id == permissionId)
                .Select(s => new PermisoEconomicoDownload
                {
                    DepartamentoAdscripcion = $"{_context.CmcatalogoProyectos.First(x => x.IdCatalogoProyecto == s.pee.IdCatalogoProyecto).DescripcionProyecto} / {_context.TurnosxCentrosDeTrabajos.Join(_context.CentrosDeTrabajos, t => t.CentroDeTrabajoId, c => c.CentroDeTrabajoId, (t, c) => new
                    {
                        t.TurnoxCentroDeTrabajoId,
                        t.Turno,
                        c.Nombre,
                        c.Clave
                    })
                    .Where(x => x.TurnoxCentroDeTrabajoId == s.pee.pe.TurnoCentroTrabajoId)
                    .Select(ts => (ts.Clave.Substring(0, 2) == "03") ? $"{ts.Nombre.Substring(45, 10)}, {(ts.Turno == 0 ? "TM" : "TV")}" : (ts.Nombre + ", " + (ts.Turno == 0 ? "TM" : "TV")))
                    .FirstOrDefault()}",
                    PuestoEmpleado = s.pee.DenominacionPlaza ?? "",
                    NombreEmpleado = $"{s.Nombres} {s.PrimerApellido} {s.SegundoApellido}",
                    FechaSolicitud = s.pee.pe.FechaSolicitud.GetValueOrDefault().ToString("dd MMMM yyyy"),
                    LugarElaboracion = _context.Localidads.First(x => x.LocalidadId == s.DomicilioLocalidad).Localidad1 ?? "",
                    LapsoPermiso = $"({s.pee.pe.LapsoPermisoDiasHabiles} {(s.pee.pe.LapsoPermisoDiasHabiles == 1 ? "día" : "días")}) {s.pee.pe.ComentarioDias}",
                    CongoceSueldo = $"{(s.pee.pe.ConGoceSueldo.GetValueOrDefault() ? "Si" : "No")}",
                    MotivoSolicitud = s.pee.pe.Comentario ?? ""
                }).FirstOrDefaultAsync();

            if (permisoEconomico is null)
                return formFieldMap;

            permisoEconomico.FirmaDigital = $"==#{GetDigitalSignature($"{permisoEconomico.NombreEmpleado}|!{permissionId}/permisoEconomico")}";

            formFieldMap = permisoEconomico.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => $"{char.ToLower(prop.Name[0])}{prop.Name.Substring(1)}", prop => prop.GetValue(permisoEconomico) as string ?? "");

            return formFieldMap;
        }

        private string GetDigitalSignature(string inputString)
        {
            byte[] hash;
            using var hmac = new HMACSHA512();
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            StringBuilder sb = new();
            foreach (byte b in hash) sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
