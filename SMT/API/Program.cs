using API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SMT.API.Configuration;
using SMT.Application.Interfaces;
using SMT.Application.Services;
using SMT.Infrastructure.Persistence;
using SMT.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);
// Define CORS policy
var allowedOrigins = "_allowedOrigins";



builder.Services.AddDbContext<SMTDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/data-protection"))
    .SetApplicationName("SMT_OMS");

builder.Services.AddSerilog(options =>
{
    options.ReadFrom.Configuration(builder.Configuration);

});

//Authorization in Swagger
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // 1. Define the Security Scheme
    opt.AddSecurityDefinition("FirebaseBearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter Firebase ID token (JWT) into field below.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // 2. Apply Security Requirement Globally
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "FirebaseBearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Added Authorization in Swaggers to test the APIs.

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Firebase ID token: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication("Firebase")
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(
        "Firebase", options => { });

builder.Services.AddAuthorization();

// Add repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();

// Add Application Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<ComponentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initialize Firebase
FirebaseConfig.InitializeFirebase();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<SMTDbContext>();
        // Apply any pending migrations and create the DB if it doesn't exist
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }


    //    var db = scope.ServiceProvider.GetRequiredService<SMTDbContext>();
    //    db.Database.EnsureCreated();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(allowedOrigins);          

app.UseAuthentication();              
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();

