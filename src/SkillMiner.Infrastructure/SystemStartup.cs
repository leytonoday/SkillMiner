using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SkillMiner.Infrastructure.BackgroundJobs;

namespace SkillMiner.Infrastructure;

public class SystemStartup
{
    private static IServiceProvider _serviceProvider = null!;

    public static async Task Start(IConfiguration configuration, IServiceCollection serviceCollection)
    {
        SetupCompositionRoot(configuration, serviceCollection);
        await SetupScheduledJobs();
    }

    public static void SetupCompositionRoot(IConfiguration configuration, IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureInfrastructureAndApplication(configuration);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    } 

    public static async Task<IScheduler> SetupScheduledJobs()
    {
        var schedulerFactory = _serviceProvider.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        await AddBackgroundJob<ProcessCommandQueueJob>(scheduler, 2);

        await scheduler.Start();

        return scheduler;
    }

    private static async Task AddBackgroundJob<TJob>(IScheduler scheduler, int intervalSeconds) where TJob : IJob
    {
        var jobName = typeof(TJob).Name;
        var job = JobBuilder.Create<TJob>().WithIdentity(jobName).Build();
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithIdentity(jobName)
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalSeconds).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}
