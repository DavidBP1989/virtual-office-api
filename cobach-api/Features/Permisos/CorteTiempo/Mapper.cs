using AutoMapper;
using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos.CorteTiempo
{
    public class Mapper : Profile
    {
        public Mapper()
        { 
            CreateMap<CortesDeTiempoResponse, CorteTiempoList.Response>();
        }
    }
}
