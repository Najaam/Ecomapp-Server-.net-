using eCommrece.SharedLibrary.DependancyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependancyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastureServices(this IServiceCollection services, IConfiguration config)
        {
            //add DB conectivity
            //add auth scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["Myserilog:Finename"]!);
            // Create a dependacy injection
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder app)
        {
            // add any middlewares here
            // Global Exception : handle External Errors
            // Lsiten only apigateway; block all outside calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }  
}
