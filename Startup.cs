using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GrabExpressApi.SDK;
using GrabExpressApi.SDK.Configuration;

namespace GrabExpressApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Grab Express API",
                    Version = "v1",
                    Description = "A .NET SDK and API wrapper for Grab Express delivery services"
                });
            });

            // Configure Grab Express SDK
            var grabConfig = new GrabExpressConfig
            {
                ClientId = Configuration["GrabExpress:ClientId"] ?? string.Empty,
                ClientSecret = Configuration["GrabExpress:ClientSecret"] ?? string.Empty,
                Environment = Configuration["GrabExpress:Environment"] ?? "staging"
            };

            services.AddSingleton(grabConfig);
            services.AddSingleton<GrabExpressClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grab Express API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
