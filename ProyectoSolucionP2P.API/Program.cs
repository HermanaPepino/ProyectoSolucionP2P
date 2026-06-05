using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infraestructure.Repositories;
using ProyectoSolucionP2P.CORE.Core.Services;
using ProyectoSolucionP2P.CORE.Infraestructure.Data;

var builder = WebApplication.CreateBuilder(args);

// connection string
var cnx = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddDbContext<CambioSeguroP2pdbContext>(options =>
    options.UseSqlServer(cnx));

// Repositories
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IVerificacionIdentidadRepository, VerificacionIdentidadRepository>();
builder.Services.AddTransient<IMensajeRepository, MensajeRepository>();
builder.Services.AddTransient<IOfertaRepository, OfertaRepository>();
builder.Services.AddTransient<IReporteAdministrativoRepository, ReporteAdministrativoRepository>();
builder.Services.AddTransient<IOperacionRepository, OperacionRepository>();
builder.Services.AddTransient<ICalificacionRepository, CalificacionRepository>();
builder.Services.AddTransient<ITemporizadorOperacionRepository, TemporizadorOperacionRepository>();
builder.Services.AddTransient<IComprobantePagoRepository, ComprobantePagoRepository>();
builder.Services.AddTransient<IDisputaRepository, DisputaRepository>();

// Services
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IVerificacionIdentidadService, VerificacionIdentidadService>();
builder.Services.AddTransient<IMensajeService, MensajeService>();
builder.Services.AddTransient<IOfertaService, OfertaService>();
builder.Services.AddTransient<IReporteAdministrativoService, ReporteAdministrativoService>();
builder.Services.AddTransient<IOperacionService, OperacionService>();
builder.Services.AddTransient<ICalificacionService, CalificacionService>();
builder.Services.AddTransient<ITemporizadorOperacionService, TemporizadorOperacionService>();
builder.Services.AddTransient<IComprobantePagoService, ComprobantePagoService>();
builder.Services.AddTransient<IDisputaService, DisputaService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
