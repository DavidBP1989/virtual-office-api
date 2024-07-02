using cobach_api.Application.Dtos.Empleado;

namespace cobach_api.Features.Empleado.Interfaces
{
    public interface IEmpleado
    {
        Task<InformacionGeneralResponse> ObtenerInformacionGeneral();
        Task<FondoAhorroResponse?> ObtenerFondoAhorro();
        Task<List<Application.Dtos.Empleado.FondoAhorroHistorial>> ObtenerFondoAhorroHistorial(int idRegistro);
    }
}
