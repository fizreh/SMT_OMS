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

// Add DbContext
//builder.Services.AddDbContext<SMTDbContext>(options =>
//    options.UseSqlite("Data Source=Data/smt.db"));

//For Docker
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
    var db = scope.ServiceProvider.GetRequiredService<SMTDbContext>();
    db.Database.EnsureCreated();
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

