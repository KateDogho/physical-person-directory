using System.IO.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PhysicalPersonDirectory.Api.Middlewares;
using PhysicalPersonDirectory.Api.OperationFilters;
using PhysicalPersonDirectory.Application;
using PhysicalPersonDirectory.Application.Services.Abstract;
using PhysicalPersonDirectory.Application.Services.Concrete;
using PhysicalPersonDirectory.Application.Validators;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;
using PhysicalPersonDirectory.Infrastructure;
using PhysicalPersonDirectory.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PhysicalPersonDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPhysicalPersonRepository, PhysicalPersonRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
builder.Services.AddScoped<IRelatedPhysicalPersonRepository, RelatedPhysicalPersonRepository>();
builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<IFileStreamService, FileStreamService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "PhysicalPersonDirectory.Application.Resources";
});

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreatePhysicalPersonValidator));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Physical Person Directory", Version = "v1" });
    options.OperationFilter<CustomHeaderOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetService<PhysicalPersonDbContext>()!.Database.Migrate();

app.Run();