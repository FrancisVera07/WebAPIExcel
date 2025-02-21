using Microsoft.EntityFrameworkCore;
using WebAPIExcel.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Almacenamiento en memoria
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("ContextList"));

// Configurar las CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .WithMethods("GET", "POST")
            .WithHeaders("Content-Type", "Authorization")
            .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar la solicitud pre-verificación OPTIONS
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();