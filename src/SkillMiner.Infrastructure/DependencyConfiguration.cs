using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillMiner.Application;
using FluentValidation;
using SkillMiner.Domain.Shared.Persistence;
using SkillMiner.Infrastructure.Persistence;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Persistence.Interceptors;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.CommandQueue;
using Quartz;
using MediatR;
using SkillMiner.Application.Abstractions.Behaviours;

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
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Auto-mapper
        services.AddAutoMapper(applicationAssembly);

        // Database services
        services.AddDatabaseServices(configuration);

        // Command queue
        services.AddScoped<ICommandQueueWriter, CommandQueueWriter>();
        services.AddScoped<ICommandQueueReader, CommandQueueReader>();

        // Application Services
    }

    private static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJobListingRepository, JobListingRepository>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>(); // Intercepts to update auditable entity properties
        services.AddSingleton<DomainEventPublisherInterceptor>(); // Intercepts to publish domain events after saving changes

        services.AddDbContext<DatabaseContext>((provider, options) =>
        {
            // Setup database context
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
