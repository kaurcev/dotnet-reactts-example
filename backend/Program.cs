using backend.Interfaces;
using backend.Repositories;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:3000";
var environment = builder.Environment;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

ConfigureServices(builder.Services, configuration, frontendUrl, environment);

var app = builder.Build();

ConfigurePipeline(app, environment, configuration);

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration,
    string frontendUrl,
    IHostEnvironment environment
)
{
    services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System
                .Text
                .Json
                .JsonNamingPolicy
                .CamelCase;
            options.JsonSerializerOptions.WriteIndented = environment.IsDevelopment();
        });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(
            "v1",
            new()
            {
                Title = "DateTime API",
                Version = "v1",
                Description =
                    "Простой API для работы с временем и датами (серверное время, UTC, записи).",
            }
        );
    });

    services.AddScoped<IDateTimeRepository, DateTimeRepository>();
    services.AddScoped<DateTimeService>();

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(frontendUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });
}

static void ConfigurePipeline(
    WebApplication app,
    IHostEnvironment environment,
    IConfiguration configuration
)
{
    if (!environment.IsDevelopment())
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    if (!environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseSwagger();
    if (environment.IsDevelopment())
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "DateTime API v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors();

    app.MapControllers();

    app.MapFallbackToFile("index.html");

    app.MapGet("/error", () => Results.Problem("Error occurred."));

    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation(
        "Веб-приложение запущено на http://localhost:{Port}",
        configuration["BackendPort"] ?? "3001"
    );
    if (environment.IsDevelopment())
    {
        logger.LogInformation(
            "Swagger UI: http://localhost:{Port}/swagger",
            configuration["BackendPort"] ?? "3001"
        );
        logger.LogInformation(
            "Swagger JSON: http://localhost:{Port}/swagger/v1/swagger.json",
            configuration["BackendPort"] ?? "3001"
        );
    }
}
