using CS2_GEO.Data;
using CS2_GEO.Endpoints;
using CS2_GEO.Game;
using CS2_GEO.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<GameDbContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("Default"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IMapRepository, MapRepository>();

builder.Services.AddSingleton<IScoringStrategy>(new ExponentialScoring(new ScoringOptions()));
builder.Services.AddSingleton<IGameScorer, GameScorer>();

builder.Services.AddScoped<IMapsService, MapsService>();
builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IRoundsService, RoundsService>();

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.UseCors("Frontend");
app.UseStaticFiles();

app.MapMapsApi();
app.MapRoundsApi();
app.MapGamesApi();

app.Run();
