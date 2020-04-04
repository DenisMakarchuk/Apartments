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
            ;
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}