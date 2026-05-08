using eCommrece.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eCommrece.SharedLibrary.DependancyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
            // add genreic database context
            services.AddDbContext<TContext>(option => option.UseSqlServer(config.GetConnectionString("eCommerceConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));
            //configure serlog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug().
                WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",
                               rollingInterval: Serilog.RollingInterval.Day).CreateLogger();

            //add jwt authentication scheme
            JWTAuthenticationScheme.AddJWTAuthenticationSchemes(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            // use global Exception handling middleware
            app.UseMiddleware<GlobalExceptionMiddleware>();
            // register middle ware for outside api calls
            app.UseMiddleware<ListenToOnlyApiGatway> ();
            return app;
        }
    }
}
