namespace cobach_api.Application.Dtos.Empleado
{
    public class FondoAhorroResponse
    {
        public int IdRegistro { get; set; }
        public DateTime FechaIngreso { get; set; }
        public double? Aportacion { get; set; }
        public bool? TipoAportacion { get; set; }
        public int? Quincena { get; set; }
        public double Acumulado { get; set; }
        public List<CuentaBancaria>? CuentasBancarias { get; set; }
        public List<Beneficiario>? Beneficiarios { get; set; }
        public List<Prestamo>? Prestamos { get; set; }

        public class Prestamo
        {
            public double? ImporteSolicitado { get; set; }
            public double DescuentoQuincenal { get; set; }
            public double ResumenSaldo { get; set; }
            public DateTime? FechaPrestamo { get; set; }
            public double? ImporteTransferencia { get; set; }
            public string? Aval { get; set; }
        }

        public class CuentaBancaria
        {
            public string? Banco { get; set; }
            public string? NumCuenta { get; set; }
            public string? NumTarjeta { get; set; }
            public string? Clabe { get; set; }
        }

        public class Beneficiario
        {
            public string? NombreBeneficiario { get; set; }
            public string? Parentesco { get; set; }
            public byte? Porcentaje { get; set; }
        }
    }
}
