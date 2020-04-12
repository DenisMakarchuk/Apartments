using System;
using Apartments.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Apartments.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config)
        {
            //configure your Data Layer services here

            services.AddDbContext<ApartmentContext>(options =>
                options.UseSqlServer(config.GetSection("ConnectionString:ApartmentConnection").Value));

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    config.GetSection("ConnectionString:IdentityConnection").Value));

            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
    }
}