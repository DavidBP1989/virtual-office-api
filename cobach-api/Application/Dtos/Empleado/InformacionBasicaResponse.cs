namespace cobach_api.Application.Dtos.Empleado
{
    public class InformacionBasicaResponse
    {
        public string Nombre { get; set; } = null!;
        public string EmpleadoId { get; set; } = null!;
        public byte[]? ProfilePhoto { get; set; }
    }
}
