using CandidatesAPI.Application;
using CandidatesAPI.Application.Interfaces;
using CandidatesAPI.Application.Services;
using CandidatesAPI.Infrastructure;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;
using CandidatesAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Candidates API",
        Version = "v1",
        Description = "API for managing contracts and exchange rates",
        Contact = new OpenApiContact
        {
            Name = "Branislav Ristivojevic",
            Email = "baneris95@gmail.com"
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CandidatesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Repositories
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IAmortPlanRepository, AmortPlanRepository>();
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
//Services
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Candidates API V1");
        options.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
