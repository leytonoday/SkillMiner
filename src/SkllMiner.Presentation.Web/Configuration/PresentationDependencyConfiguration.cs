namespace SkillMiner.Presentation.Web.Configuration;

public static class PresentationDependencyConfiguration
{
    public static void ConfigurePresentation(this IServiceCollection services) 
    {
        // Add controllers with configuration
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        });

        // Configure API versioning
        services.AddApiVersioning();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
