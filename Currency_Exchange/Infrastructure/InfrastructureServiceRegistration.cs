using Infrastructure.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Application.Contracts.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Persistence;
using System.Reflection;
using Application.API_Calls;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories.Persistence.NewFolder;

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
            services.AddScoped<IAdminServices,AdminServices>();
            services.AddScoped<IUserServices,UserServices>();
            services.AddTransient<IApiServices, CurrencyService>();
            services.AddTransient<ITestTransaction, TestTransactionService>();
           

            return services;

        }


    }
}
