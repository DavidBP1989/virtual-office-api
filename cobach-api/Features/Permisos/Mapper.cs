using AutoMapper;
using cobach_api.Application.Dtos.Permisos;

namespace cobach_api.Features.Permisos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<PermisosPorEmpleado, PermisoPorEmpleado.Response>();
        }
    }
}
