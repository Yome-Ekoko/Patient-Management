using Patient_Management.Domain.Settings;
using Patient_Management.Infrastructure.Middleware;
using Patient_Management.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Patient_Management.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Patient_Management Admin API");
                setupAction.RoutePrefix = "OpenAPI";
            });
        }

        public static void ConfigureCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            AdminOptions adminOptions = new AdminOptions();
            configuration.Bind("AdminOptions", adminOptions);

            app.UseCors(options =>
                 options.WithOrigins(adminOptions.AllowedHosts)
                 .AllowAnyHeader()
                 .WithExposedHeaders("Content-Disposition")
                 .AllowAnyMethod());
        }

        public async static void ConfigureDbContext(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
        }

        
    }
}
