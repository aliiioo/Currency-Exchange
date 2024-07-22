using Application.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;


namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            #region Quartz

            services.AddQuartz(option =>
                {
                    option.UseMicrosoftDependencyInjectionJobFactory();
                    var jobKey = JobKey.Create(nameof(TimeScheduling));
                    option
                        .AddJob<TimeScheduling>(jobKey)
                        .AddTrigger(trigger =>
                            trigger
                                .ForJob(jobKey)
                                .WithSimpleSchedule(s =>
                                    s.WithIntervalInMinutes(10).RepeatForever()));
                }
            );
            services.AddQuartzHostedService();

            #endregion

            return services;
        }

    }
}
