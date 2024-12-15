﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillMiner.Application;
using FluentValidation;
using SkillMiner.Domain.Shared.Persistence;
using SkillMiner.Infrastructure.Persistence;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Persistence.Interceptors;

namespace SkillMiner.Infrastructure;

public static class DependencyRegistration
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(ApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(DependencyRegistration).Assembly;
        
        services.AddSingleton(configuration);

        // Mediator
        services.AddMediatR(config => config.RegisterServicesFromAssembly(applicationAssembly));

        // Validation
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Auto-mapper
        services.AddAutoMapper(applicationAssembly);

        // Database services
        services.AddDatabaseServices(configuration);

        // Application Services
    }

    private static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IJobListingRepository, JobListingRepository>();

        services.AddDbContext<DatabaseContext>((provider, options) =>
        {
            // Setup database context
            options.UseSqlServer(configuration.GetConnectionString("SkillMiner"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(DependencyRegistration).Assembly.GetName().Name);
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
