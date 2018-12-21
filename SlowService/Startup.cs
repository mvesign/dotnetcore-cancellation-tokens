using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SlowService.Filters;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace SlowService
{
    /// <summary>
    /// Class containing details of the start-up for the web API.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Create an instance to start-up the web API.
        /// </summary>
        /// <param name="configuration">Details of the web API configuration settings.</param>
        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Details of the web API configuration settings.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure services for the web API.
        /// </summary>
        /// <param name="services">Details of the existing services.</param>
        [UsedImplicitly]
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            services
                .AddCors()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Slow Service", Version = "v1" });
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "SlowService.xml"));
                    c.OperationFilter<ExamplesOperationFilter>();
                })
                .AddMvc(options =>
                {
                    options.Filters.Add<OperationCancelledExceptionFilter>();
                });
        }

        /// <summary>
        /// Configure the web API.
        /// </summary>
        /// <param name="application">Details of the application builder.</param>
        /// <param name="environment">Details of the hosting environment.</param>
        [UsedImplicitly]
        public void Configure(
            IApplicationBuilder application,
            IHostingEnvironment environment
        )
        {
            application
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voice FIAT API");
                    c.RoutePrefix = "docs";
                })
                .UseMvc();
        }
    }
}