using DevEncurtaUrl.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// PARA ACESSO AO BANCO EM MEMÓRIA
// builder.Services.AddDbContext<DevEncurtaUrlDbContext>(o => o.UseInMemoryDatabase("DevEncurtaUrlDb"));

// PARA ACESSO AO SQL Server
// var connectionString = builder.Configuration.GetConnectionString("DevEncurtaUrlCs");
// builder.Services.AddDbContext<DevEncurtaUrlDbContext>(o => o.UseSqlServer(connectionString));

// PARA ACESSO AO SQLite
var connectionString = builder.Configuration.GetConnectionString("DevEncurtaUrlCs");
builder.Services.AddDbContext<DevEncurtaUrlDbContext>(o => o.UseSqlite(connectionString));

// TODO: Para usar com angular
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevEncurtaUrl.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Samuel B. Oldra",
            Email = "samuel.oldra@gmail.com",
            Url = new Uri("https://github.com/samuel-oldra")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Serilog
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()

        // PARA LOG NO SQL Server
        // .WriteTo.MSSqlServer(
        //     connectionString,
        //     sinkOptions: new MSSqlServerSinkOptions()
        //     {
        //         AutoCreateSqlTable = true,
        //         TableName = "Logs"
        //     })

        // PARA LOG NO SQLite
        .WriteTo.SQLite(Environment.CurrentDirectory + @"\Data\dados.db")

        // PARA LOG NO CONSOLE
        .WriteTo.Console()

        .CreateLogger();
}).UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
// INFO: Swagger visível só em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        // TODO: Remover /swagger para acessar o SWAGGER
        o.RoutePrefix = "swagger";
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "DevEncurtaUrl.API v1");
    });
}

app.UseCors(); // TODO: Para usar com angular

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();