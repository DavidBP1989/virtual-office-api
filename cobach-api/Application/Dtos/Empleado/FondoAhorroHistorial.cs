namespace cobach_api.Application.Dtos.Empleado
{
    public class FondoAhorroHistorial
    {
        public string Concepto { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string Quincena { get; set; }
        public double Importe { get; set; }
    }
}
