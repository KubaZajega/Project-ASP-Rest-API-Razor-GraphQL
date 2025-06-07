using Jakub_Zajega_14987.Application.Contracts.Identity;
using Jakub_Zajega_14987.Application.Contracts.Persistence;
using Jakub_Zajega_14987.Infrastructure.Identity;
using Jakub_Zajega_14987.Infrastructure.Persistence;
using Jakub_Zajega_14987.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jakub_Zajega_14987.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("JakubZajegaDb"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}