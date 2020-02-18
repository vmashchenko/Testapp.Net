using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation.AspNetCore;

using TestApp.Services;
using TestAppApi.Infrastructure.Extensions;
using TestAppApi.Infrastructure.Security;
using System.Text.Json;

namespace TestAppApi
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
            // add options services to the DI container
            services.AddOptions();

            // add Cross-Origin Requests
            services.AddCors();

            // add Swagger
            services.AddSwagger();

            services.AddMvc(options =>
            {
                Debug.WriteLine("MVC options:");
                Debug.WriteLine(options);
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;                    
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest)
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                fv.RegisterValidatorsFromAssemblyContaining<FileVmValidator>();
            });

            // add EntityFramework DbContext
            services.AddAppDbContext(Configuration);

            services.AddControllers();

            // register AutoMapper for TestApp.Services
            services.AddAutoMapper(typeof(ServiceBase).Assembly);

            // add ability to get access for HttpContext
            services.AddHttpContextAccessor();

            // add ability to use HttpClient class
            services.AddHttpClient();

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // register all other needed services
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UpdateDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // use Swagger on Development mode only
                app.UseAppSwagger();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.ConfigureExceptionHandler();

            // use SeriLog, see https://serilog.net/
            app.AddSerilogFileLogger(loggerFactory, Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
