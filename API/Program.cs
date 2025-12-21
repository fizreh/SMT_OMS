using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Services;
using SMT.Infrastructure.Persistence;
using SMT.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<SMTDbContext>(options =>
    options.UseSqlite("Data Source=Data/smt.db"));


// Add repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();

// Add Application Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<ComponentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SMTDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();