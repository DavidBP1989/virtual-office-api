using AutoMapper;
using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos.PermisoEconomico
{
    public class Mapper : Profile
    {
        public Mapper()
        { 
            CreateMap<PermisoEconomicoResponse, PermisoEconomicoList.Response>();
        }
    }
}
