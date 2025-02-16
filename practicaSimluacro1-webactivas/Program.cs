using Microsoft.EntityFrameworkCore;
using practicaSimluacro1_webactivas.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

//Inyeccion por dependencia del string de conexion al contexto
builder.Services.AddDbContext<bibliotecaContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("bibliotecaConnection")
        )
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
