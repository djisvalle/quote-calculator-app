using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Application.Interfaces;
using QuoteCalculator.Application.Mappings;
using QuoteCalculator.Application.Services;
using QuoteCalculator.Domain.Repositories;
using QuoteCalculator.Infrastracture;
using QuoteCalculator.Infrastracture.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();

builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddScoped<IBlacklistRepository, BlacklistRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IGlobalConfigurationRepository, GlobalConfigurationRepository>();

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => { },
    typeof(ApplicantMappingProfile),
    typeof(LoanApplicationMappingProfile),
    typeof(ProductTypeMappingProfile));

// Register DbContext
builder.Services.AddDbContext<QuoteCalculatorDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("QuoteCalculatorDatabase")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("loan-app", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
    );
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuoteCalculatorDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("loan-app");

app.UseAuthorization();

app.MapControllers();

app.Run();
