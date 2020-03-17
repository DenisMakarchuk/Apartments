using System;
using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            //configure your Data Layer services here
            return services;
        }
    }
}