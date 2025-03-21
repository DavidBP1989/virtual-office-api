using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class InformacionLaboral
{
    public string InformacionLaboralId { get; set; } = null!;

    public string EmpleadoId { get; set; } = null!;

    public byte IdPlaza { get; set; }

    public string? DenominacionPlaza { get; set; }

    /// <summary>
    /// 1.-Administrativo
    /// 2.-Docente
    /// </summary>
    public byte TipoEmpleado { get; set; }

    public int TurnoxCentroDeTrabajoId { get; set; }

    public string Categoria { get; set; } = null!;

    /// <summary>
    /// 1.-Base
    /// 2.-Confianza
    /// 3.-Interino
    /// </summary>
    public byte Caracter { get; set; }

    /// <summary>
    /// 0.-DEFINITIVO
    /// 1.-INTERINO
    /// 2.-TIEMPO DETERMINADO
    /// 3.-TIEMPO INDETERMINADO
    /// </summary>
    public byte? SubCaracter { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaBaja { get; set; }

    public string? MotivoBaja { get; set; }

    /// <summary>
    /// Valores numéricos para tipos de baja, desde jubilacion, renuncia, deceso o simple baja
    /// </summary>
    public byte Status { get; set; }

    public byte Sindicalizado { get; set; }

    public DateTime? FechaRegistroNss { get; set; }

    public DateTime? FechaModificacionNss { get; set; }

    public int? IdCatalogoProyecto { get; set; }

    public bool? K1 { get; set; }

    public byte? HorasK1 { get; set; }

    /// <summary>
    /// Campo para aplicar un borrado lógico. 0 = activo 1 = Borrado logico
    /// </summary>
    public byte? StatusBorrado { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual TurnosxCentrosDeTrabajo TurnoxCentroDeTrabajo { get; set; } = null!;
}
