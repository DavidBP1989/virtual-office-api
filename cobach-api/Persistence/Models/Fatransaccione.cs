using System;
using System.Collections.Generic;

namespace cobach_api.Persistence.Models;

public partial class Fatransaccione
{
    public int IdTransaccion { get; set; }

    public int IdRegistroFa { get; set; }

    public int UsuarioId { get; set; }

    public int IdCatalogoConcepto { get; set; }

    public int? Quincena { get; set; }

    public DateTime FechaTransaccion { get; set; }

    public double Importe { get; set; }

    public int Ejercicio { get; set; }

    public byte? Estado { get; set; }

    public int? IdPrestamo { get; set; }

    public virtual Faprestamo? IdPrestamoNavigation { get; set; }

    public virtual Faregistro IdRegistroFaNavigation { get; set; } = null!;
}
