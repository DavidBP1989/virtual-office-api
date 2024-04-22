using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Faprestamo
{
    public int IdPrestamo { get; set; }

    public int IdRegistroFa { get; set; }

    public double? ImporteSolicitado { get; set; }

    public double? ImporteTransferencia { get; set; }

    public double? Intereses { get; set; }

    public double? Amortizacion { get; set; }

    public double? ComisionBanco { get; set; }

    public string Aval { get; set; } = null!;

    public int? Plazo { get; set; }

    public int Quincena { get; set; }

    public DateTime? FechaPrestamo { get; set; }

    public byte? Estado { get; set; }

    public int? IdPrestamoSaldado { get; set; }

    public double DescuentoInicial { get; set; }

    public double DescuentoQuincenal { get; set; }

    public int ResumenDescuentos { get; set; }

    public double ResumenSaldo { get; set; }

    public virtual Empleado AvalNavigation { get; set; } = null!;

    public virtual ICollection<Fatransaccione> Fatransacciones { get; set; } = new List<Fatransaccione>();

    public virtual Faprestamo? IdPrestamoSaldadoNavigation { get; set; }

    public virtual Faregistro IdRegistroFaNavigation { get; set; } = null!;

    public virtual ICollection<Faprestamo> InverseIdPrestamoSaldadoNavigation { get; set; } = new List<Faprestamo>();
}
