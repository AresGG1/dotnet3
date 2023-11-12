using AutoMapper;
using BLL.Configurations;
using BLL.Interfaces.Services;
using BLL.Services;
using BLL.Validation.Requests;
using DAL;
using DAL.Cache;
using DAL.Data;
using DAL.Data.Repositories;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using lab3.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

string connectionString = 
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<AircraftRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PilotAircraftRequestValidator>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddSingleton<CustomMemoryCache>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "";
});


builder.Services.AddDbContext<AirportDatabaseContext>(
    options => options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
        )
    );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AircraftRepository>();
builder.Services.AddScoped<IAircraftRepository, CachedAircraftRepository>();
builder.Services.AddScoped<PilotRepository>();
builder.Services.AddScoped<IPilotRepository, CachedPilotRepository>();

builder.Services.AddScoped<IAircraftService, AircraftService>();
builder.Services.AddScoped<IPilotService, PilotService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

    
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
