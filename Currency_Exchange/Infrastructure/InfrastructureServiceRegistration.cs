using Infrastructure.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Application.Contracts.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Persistence;
using System.Reflection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CurrencyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IMessageSender, MessageSender>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<ICurrencyServices, CurrencyServices>();
            services.AddScoped<IOthersAccountServices, OthersAccountServices>();
            services.AddScoped<IProviderServices,ProviderServices>();

            return services;

        }


    }
}
