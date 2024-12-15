using Microsoft.Extensions.DependencyInjection;
using SkillMiner.Infrastructure;
using SkillMiner.Infrastructure.BackgroundJobs;
using SkillMiner.Presentation.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

await SystemStartup.Start(builder.Configuration, builder.Services);
builder.Services.ConfigurePresentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
