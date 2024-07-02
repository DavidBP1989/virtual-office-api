using AutoMapper;
using cobach_api.Application.Dtos.Empleado;

namespace cobach_api.Features.Empleado
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<InformacionGeneralResponse, InformacionGeneral.Response>();
            CreateMap<FondoAhorroResponse, FondoAhorro.Response>();
            CreateMap<Application.Dtos.Empleado.FondoAhorroHistorial, FondoAhorroHistorial.Response>();
        }
    }
}
