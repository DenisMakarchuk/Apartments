using System;
using Microsoft.Extensions.DependencyInjection;
using Apartments.Data;
using Microsoft.Extensions.Configuration;

namespace Apartments.Domain.Logic
{
    public static class DomainLogicExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataServices(config);
            //configure your Domain Logic Layer services here
            return services;
        }
    }
}