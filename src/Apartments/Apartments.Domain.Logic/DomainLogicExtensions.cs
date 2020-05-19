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
using Apartments.Data.Context;
using Microsoft.AspNetCore.Identity;
using Apartments.Domain.Logic.Images.ImageInterfaces;
using Apartments.Domain.Logic.Images.ImageServices;
using Apartments.Domain.Users;
using Apartments.Domain.Logic.Email;

namespace Apartments.Domain.Logic
{
    public static class DomainLogicExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataServices(config);

            services.AddAutoMapper(typeof(MapperLogicModule));

            services.AddScoped<ICommentAdministrationService, CommentAdministrationService>();
            services.AddScoped<IIdentityUserAdministrationService, IdentityUserAdministrationService>();

            services.AddScoped<IApartmentSearchService, ApartmentSearchService>();

            services.AddScoped<IApartmentUserService, ApartmentUserService>();
            services.AddScoped<ICommentUserService, CommentUserService>();
            services.AddScoped<IOrderUserService, OrderUserService>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();

            services.AddScoped<IUserAdministrationService, UserAdministrationService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IImageWriter, ImageWriter>();
            services.AddScoped<IExistsImahesOperator, ExistsImahesOperator>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailConfirmation, EmailConfirmation>();

            services.Configure<AuthMessageSenderOptions>(config);

            return services;
        }
    }
}