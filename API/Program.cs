using Core.Helpers.Decoders;
using Core.Interfaces.Decoders;
using Core.Interfaces.Repositories;
using Core.Profiles;
using Core.Repositories;
using Database.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Debug("Application Starting Up");

    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

    // Configure AutoMapper
    builder.Services.AddAutoMapper(options =>
    {
        options.AddProfile<SensorProfile>();
        options.AddProfile<SensorTypeProfile>();
        options.AddProfile<Bme280MeasurementProfile>();
    });

    // Configure API Version
    builder.Services.AddApiVersioning(option =>
    {
        option.DefaultApiVersion = new ApiVersion(1, 0);
        option.AssumeDefaultVersionWhenUnspecified = true;
        option.ReportApiVersions = true;
    });

    // Add services to the container.
    builder.Services.AddControllers()
        .AddNewtonsoftJson();
    builder.Services.AddCors();
    // Configure Database
    builder.Services.AddDbContext<LoRaContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("LoRaWANConnection"));
    });
    // Add Repositories
    builder.Services.AddScoped<ISensorRepository, SensorRepository>();
    builder.Services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
    builder.Services.AddScoped<IBme280MeasurementRepository, Bme280MeasurementRepository>();
    // Add Decoders
    builder.Services.AddScoped<IBme280Decoder, Bme280Decoder>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(options =>
    {
        options.AllowAnyOrigin();
        options.AllowAnyMethod();
        options.AllowAnyHeader();
    });


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}