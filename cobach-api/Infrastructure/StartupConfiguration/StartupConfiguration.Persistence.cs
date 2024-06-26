﻿using cobach_api.Features.Empleado;
using cobach_api.Features.Empleado.Interfaces;
using cobach_api.Features.Seguridad;
using cobach_api.Features.Seguridad.Interfaces;
using cobach_api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cobach_api.Infrastructure.StartupConfiguration
{
    public static partial class StartupConfiguration
    {
        public static void RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SiiaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("siia"))
            );

            services.AddScoped<IAuthentication, SeguridadRepository>();
            services.AddScoped<IEmpleado, Repository>();
        }
    }
}
