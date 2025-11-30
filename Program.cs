using learning_center_webapi.Contexts.IAM.Application.CommandServices;
using learning_center_webapi.Contexts.IAM.Application.QueryServices;
using learning_center_webapi.Contexts.IAM.Application.ACL;
using learning_center_webapi.Contexts.IAM.Domain.Infraestructure;
using learning_center_webapi.Contexts.IAM.Infraestructure;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.ACL;
using learning_center_webapi.Contexts.Profiles.Application.CommandServices;
using learning_center_webapi.Contexts.Profiles.Application.QueryServices;
using learning_center_webapi.Contexts.Profiles.Application.ACL;
using learning_center_webapi.Contexts.Profiles.Domain.Infraestructure;
using learning_center_webapi.Contexts.Profiles.Infraestructure;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.ACL;
using learning_center_webapi.Contexts.Reminders.Application.CommandServices;
using learning_center_webapi.Contexts.Reminders.Application.QueryServices;
using learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;
using learning_center_webapi.Contexts.Reminders.Infraestructure;
using learning_center_webapi.Contexts.Claims.Application.CommandServices;
using learning_center_webapi.Contexts.Claims.Application.QueryServices;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;
using learning_center_webapi.Contexts.Claims.Infraestructure;
using learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;
using learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;
using learning_center_webapi.Contexts.RegisteredObjects.Application.ACL;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;
using learning_center_webapi.Contexts.RegisteredObjects.Infraestructure;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.ACL;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using learning_center_webapi.Contexts.Shared.Interfaces.Middleware;
using learning_center_webapi.Contexts.Teleconsultations.Application.CommandServices;
using learning_center_webapi.Contexts.Teleconsultations.Application.QueryServices;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Infraestructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration - Permitir requests desde el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;

                try
                {
                    var uri = new Uri(origin);
                    // Permitir localhost para desarrollo
                    if (uri.Host == "localhost" || uri.Host == "127.0.0.1")
                        return true;

                    // Permitir dominios de Render, Vercel, Netlify para producción
                    if (uri.Host.EndsWith(".onrender.com") ||
                        uri.Host.EndsWith(".vercel.app") ||
                        uri.Host.EndsWith(".netlify.app"))
                        return true;

                    return false;
                }
                catch
                {
                    return false;
                }
            })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("learningCenter")
                       ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'learningCenter'.");

builder.Services.AddDbContext<LearningCenterContext>(options =>
{
    options.UseMySQL(connectionString);

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
});

// Dependency injection IAM
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthCommandService>();
builder.Services.AddScoped<UserQueryService>();

// ACL Facades
builder.Services.AddScoped<IUserFacade, UserFacade>();
builder.Services.AddScoped<IProfileFacade, ProfileFacade>();
builder.Services.AddScoped<IRegisteredObjectFacade, RegisteredObjectFacade>();

// Dependency injection Teleconsultations
builder.Services.AddScoped<ITeleconsultationRepository, TeleconsultationRepository>();
builder.Services.AddScoped<TeleconsultationCommandService>();
builder.Services.AddScoped<TeleconsultationQueryService>();

// Dependency injection Profiles
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ProfileCommandService>();
builder.Services.AddScoped<ProfileQueryService>();

// Dependency injection Reminders
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<ReminderCommandService>();
builder.Services.AddScoped<ReminderQueryService>();

// Dependency injection Claims
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimCommandService, ClaimCommandService>();
builder.Services.AddScoped<IClaimQueryService, ClaimQueryService>();

// Dependency injection RegisteredObjects
builder.Services.AddScoped<IRegisteredObjectRepository, RegisteredObjectRepository>();
builder.Services.AddScoped<IRegisteredObjectCommandService, RegisteredObjectCommandService>();
builder.Services.AddScoped<IRegisteredObjectQueryService, RegisteredObjectQueryService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Crear tablas si no existen
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LearningCenterContext>();

    Console.WriteLine("Verificando y creando tablas si no existen...");
    context.Database.EnsureCreated();

    Console.WriteLine("Base de datos lista");
}

// Configure the HTTP request pipeline.
// Swagger habilitado en todos los ambientes
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

// ⭐ IMPORTANTE: Usar el middleware de excepciones ANTES de otros middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Usar CORS (DEBE IR ANTES de UseAuthorization)
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();