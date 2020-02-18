using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;

using Microsoft.EntityFrameworkCore;
using TestApp.DB;

namespace TestAppApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseAppSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApp API");
                c.DocExpansion(DocExpansion.None);
            });
        }

        /// <summary>
        /// Registers SeriLog file logger based on 'Logging' section in appsettings.json
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        public static void AddSerilogFileLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var section = configuration.GetSection("Logging");
            bool exist = section.Exists();
            if (!exist)
            {
                throw new InvalidOperationException("Logging section does not exist");
            }

            loggerFactory.AddFile(section);
        }

        /// <summary>
        /// Migrates database to the latest version.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            return app;
        }
    }
}
