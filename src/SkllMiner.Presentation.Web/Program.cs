using SkillMiner.Infrastructure;
using SkillMiner.Presentation.Web.Configuration;
using SkillMiner.Presentation.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

await SystemStartup.Start(builder.Configuration, builder.Services);
builder.Services.ConfigurePresentation();

var app = builder.Build();

app.ConfigureExceptionHander();

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
