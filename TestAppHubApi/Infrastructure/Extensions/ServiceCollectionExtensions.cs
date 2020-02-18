using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TestApp.DB;
using TestApp.DB.Repositories;
using TestApp.Domain;
using TestApp.Services;
using TestApp.Services.User;
using TestAppApi.Infrastructure.Filters;
using TestAppApi.Infrastructure.Identity;

namespace TestAppApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Constants

        public const string Version = "v1";
        public const string SwaggerDocTitle = "TestAppApi API";
        public const string SwaggerDocDescription = "TestAppApi API v1.0";

        #endregion   

        public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString(AppDbContext.ConnectionStringName);
                if (connectionString == null)
                {
                    throw new ArgumentNullException("DB connection string not found: " + AppDbContext.ConnectionStringName);
                }

                options.UseSqlServer(connectionString, action =>
                {
                    action.CommandTimeout(60);
                });

                options.EnableSensitiveDataLogging();

                // use no tracking by default
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc(Version, new OpenApiInfo
                {
                    Version = Version,
                    Title = SwaggerDocTitle,
                    Description = SwaggerDocDescription,
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);

                opts.DocumentFilter<SwaggerFilterOutControllers>();
                //opts.OperationFilter<AddRequiredHeaderParameter>();

                opts.TagActionsBy(api =>
                {
                    // group api methods by first path segment after "api/" prefix
                    return new[] { Regex.Match(api.RelativePath, @"api/([-\w]+)").Groups[1].Value };
                });
                opts.OrderActionsBy(api => api.RelativePath);
            });
        }

        public static void RegisterServices(this IServiceCollection services)
        {            
            services.AddRepositories();
            services.AddServices();

            // common                        
            services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();                                    

            services.AddTransient(typeof(Lazy<>), typeof(ServiceResolvingLazy<>));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(RepositoryBase<>));
            //services.AddTransient(typeof(ILogInboundRepository), typeof(LogInboundRepository));
            foreach (var type in typeof(FilleRepository).Assembly.ExportedTypes.Where(t => !t.IsGenericType))
            {
                foreach (var repositoryInterface in type.GetInterfaces().Where(i => i.Name.Contains("Repository")))
                {
                    services.AddTransient(repositoryInterface, type);
                }
            }
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddConventionalServices(typeof(ServiceBase).Assembly);

            return services;
        }

        /// <summary>
        /// Register all classes from <paramref name="assembly"/> matching the following pattern
        /// <code>
        /// class Abracadabra: IAbracadabra {}
        /// </code>
        /// </summary>
        private static IServiceCollection AddConventionalServices(this IServiceCollection services, Assembly assembly)
        {
            foreach (var type in assembly.ExportedTypes.Where(t => !t.IsGenericType))
            {
                foreach (var typeInterface in type.GetInterfaces().Where(i => i.Name == $"I{type.Name}"))
                {
                    services.AddTransient(typeInterface, type);
                }
            }

            return services;
        }

    }
}
