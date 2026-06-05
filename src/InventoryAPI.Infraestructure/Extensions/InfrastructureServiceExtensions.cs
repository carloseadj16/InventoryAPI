using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Infraestructure.Persistence.Context;
using InventoryAPI.Infraestructure.Repositories.Read;
using InventoryAPI.Infraestructure.Repositories.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Infraestructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddIfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("default")));

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<IInventoryMovWriteRepository, InventoryMovWriteRepository>();
            services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();

            return services;
        }
    }
}
