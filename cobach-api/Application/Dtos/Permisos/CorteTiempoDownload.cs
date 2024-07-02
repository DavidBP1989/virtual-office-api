namespace cobach_api.Application.Dtos.Permisos
{
    public class CorteTiempoDownload
    {
        public string DepartamentoAdscripcion { get; set; } = null!;
        public string NombreEmpleado { get; set; } = null!;
        public string FechaSolicitud { get; set; } = null!;
        public string HoraSalida { get; set; } = null!;
        public string TiempoEstimado { get; set; } = null!;
        public string TiempoReal { get; set; } = null!;
        public string ObjetivoCorteTiempo { get; set; } = null!;
        public string FirmaDigital { get; set; } = null!;
        public string ImagenQR { get; set; } = null!;
    }
}
