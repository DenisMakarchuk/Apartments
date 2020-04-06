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
using Apartments.Web.Validation;
using Apartments.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


            services.AddDomainServices(Configuration);
            
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

                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer",new string[0] }
                    };

                    document.SecurityDefinitions.Add("Bearer", new NSwag.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the bearer scheme",
                        Name = "Authorization",
                        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                        Type = NSwag.OpenApiSecuritySchemeType.ApiKey
                    });

                    document.Security.Add(security as NSwag.OpenApiSecurityRequirement);
                };
            });

            services.AddControllers()
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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi().UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}