using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Mantener nombres de propiedades tal cual (no camelCase)
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



// Asignacion de ConnectionString a los DBContext
builder.Services.AddDbContext<DBC_admon_sueldos>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

builder.Services.AddDbContext<DBC_usuarios>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

builder.Services.AddDbContext<DBC_incrementos>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

builder.Services.AddDbContext<DBC_Empleados>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

builder.Services.AddDbContext<DBC_resultados>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

builder.Services.AddDbContext<DBC_porcentajes_estandar>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL2022")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

// Solo redirigir a HTTPS en producción
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
