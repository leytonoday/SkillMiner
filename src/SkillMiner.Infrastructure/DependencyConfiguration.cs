using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillMiner.Application;
using FluentValidation;
using SkillMiner.Domain.Shared.Persistence;
using SkillMiner.Infrastructure.Persistence;
using SkillMiner.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Persistence.Interceptors;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.CommandQueue;
using Quartz;
using SkillMiner.Application.Abstractions.Behaviours;
using SkillMiner.Infrastructure.WebScrapers.JobListingWebScraper;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Infrastructure.WebScrapers.WebScraperHelper;

namespace SkillMiner.Infrastructure;

public static class DependencyConfiguration
{
    public static void ConfigureInfrastructureAndApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(ApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(DependencyConfiguration).Assembly;
        
        services.AddSingleton(configuration);

        // Quartz
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });

        // Mediator
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies([infrastructureAssembly, applicationAssembly]);
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        // Validation
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Auto-mapper
        services.AddAutoMapper(applicationAssembly);

        // Database services
        services.AddDatabaseServices(configuration);

        // Command queue
        services.AddScoped<ICommandQueueForProducer, CommandQueueForWriter>();
        services.AddScoped<ICommandQueueForConsumer, CommandQueueForConsumer>();

        // Application Services
        services.AddScoped<IJobListingWebScraper<MicrosoftJobListing>, MicrosoftJobListingWebScraper>();

        // Infrastructure Services
        services.AddScoped<IWebScraperHelper, WebScraperHelper>();
    }

    private static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IMicrosoftJobListingRepository, MicrosoftJobListingRepository>();

        // Interceptors
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>(); // Intercepts to update auditable entity properties
        services.AddSingleton<DomainEventPublisherInterceptor>(); // Intercepts to publish domain events after saving changes

        // Database context
        services.AddDbContext<DatabaseContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SkillMiner"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(DependencyConfiguration).Assembly.GetName().Name);
            });

            options.EnableDetailedErrors();

            var databaseContext = new DatabaseContext(options.Options as DbContextOptions<DatabaseContext>);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = databaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
            {
                try
                {
                    databaseContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventPublisherInterceptor>());
        });
    }
}
