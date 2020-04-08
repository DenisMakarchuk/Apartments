using System;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apartments.Data;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Apartments.Domain.Logic.Admin.AdminService;
using Apartments.Domain.Logic.Search.SearchServiceInterfaces;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Logic.Search.SearchServices;
using Apartments.Domain.Logic.Users.UserService;

namespace Apartments.Domain.Logic
{
    public static class DomainLogicExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataServices(config);
            //configure your Domain Logic Layer services here

            services.AddAutoMapper(typeof(MapperLogicModule));

            services.AddScoped<IIdentityUserService, IdentityUserService>();
            services.AddScoped<IIdentityUserAdministrationService, IdentityUserAdministrationService>();

            

            services.AddScoped<IUserAdministrationService, UserAdministrationService>();
            services.AddScoped<ICommentAdministrationService, CommentAdministrationService>();

            services.AddScoped<IApartmentSearchService, ApartmentSearchService>();

            services.AddScoped<IApartmentUserService, ApartmentUserService>();
            services.AddScoped<ICommentUserService, CommentUserService>();
            services.AddScoped<IOrderUserService, OrderUserService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}