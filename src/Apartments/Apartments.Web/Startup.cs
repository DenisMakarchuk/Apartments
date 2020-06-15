using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Apartments.Domain.Logic;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using Apartments.Domain.Logic.Options;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Logic.Users.UserService;
using Apartments.Domain.Logic.Validation;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Apartments.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomainServices(Configuration);

            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(a => 
            { 
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(b=>
                {
                    b.SaveToken = true;
                    b.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });

            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "RENT APARTMENTS";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Denis Makarchuk",
                        Email = string.Empty,
                        Url = "https://www.linkedin.com/in/denis-makarchuk-1816b0177/"
                    };
                };

                config.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the bearer scheme",
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.ApiKey
                    }));
                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            });

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssemblyContaining<AddApartmentValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AddCommentValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AddOrderValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AddUserValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ApartmentViewValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CommentDTOAdministrationValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CommentDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<OrderDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<UserDTOValidator>();
                });

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi().UseSwaggerUi3();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            MyIdentityDataInitializer.SeedUsers(userManager);

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Resources")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                    RequestPath = "/Resources"
            });

            app.UseCors("CorsPolicy");
            //app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}