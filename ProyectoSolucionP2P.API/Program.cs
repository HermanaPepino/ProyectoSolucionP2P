using Microsoft.EntityFrameworkCore;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var cnx = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<CambioSeguroP2pdbContext>(options =>
    options.UseSqlServer(cnx));

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddScoped<ProyectoSolucionP2P.CORE.Core.Interfaces.IUsuarioRepository,
    ProyectoSolucionP2P.CORE.Infrastructure.Repositories.UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService,
    ProyectoSolucionP2P.CORE.Core.Services.UsuarioService>();
builder.Services.AddScoped<IMonedaRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.MonedaRepository>();
builder.Services.AddScoped<IMonedaService, ProyectoSolucionP2P.CORE.Core.Services.MonedaService>();
builder.Services.AddScoped<IMetodoPagoRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.MetodoPagoRepository>();
builder.Services.AddScoped<IMetodoPagoService, ProyectoSolucionP2P.CORE.Core.Services.MetodoPagoService>();
builder.Services.AddScoped<IVerificacionIdentidadRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.VerificacionIdentidadRepository>();
builder.Services.AddScoped<IVerificacionIdentidadService, ProyectoSolucionP2P.CORE.Core.Services.VerificacionIdentidadService>();
builder.Services.AddScoped<IReporteAdministrativoRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.ReporteAdministrativoRepository>();
builder.Services.AddScoped<IReporteAdministrativoService, ProyectoSolucionP2P.CORE.Core.Services.ReporteAdministrativoService>();
builder.Services.AddScoped<IOfertaRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.OfertaRepository>();
builder.Services.AddScoped<IOfertaService, ProyectoSolucionP2P.CORE.Core.Services.OfertaService>();
builder.Services.AddScoped<IOfertaMetodoPagoRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.OfertaMetodoPagoRepository>();
builder.Services.AddScoped<IOfertaMetodoPagoService, ProyectoSolucionP2P.CORE.Core.Services.OfertaMetodoPagoService>();
builder.Services.AddScoped<IOperacionRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.OperacionRepository>();
builder.Services.AddScoped<IOperacionService, ProyectoSolucionP2P.CORE.Core.Services.OperacionService>();
builder.Services.AddScoped<ITemporizadorOperacionRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.TemporizadorOperacionRepository>();
builder.Services.AddScoped<ITemporizadorOperacionService, ProyectoSolucionP2P.CORE.Core.Services.TemporizadorOperacionService>();
builder.Services.AddScoped<IComprobantePagoRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.ComprobantePagoRepository>();
builder.Services.AddScoped<IComprobantePagoService, ProyectoSolucionP2P.CORE.Core.Services.ComprobantePagoService>();
builder.Services.AddScoped<IDisputaRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.DisputaRepository>();
builder.Services.AddScoped<IDisputaService, ProyectoSolucionP2P.CORE.Core.Services.DisputaService>();
builder.Services.AddScoped<IEvidenciaDisputaRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.EvidenciaDisputaRepository>();
builder.Services.AddScoped<IEvidenciaDisputaService, ProyectoSolucionP2P.CORE.Core.Services.EvidenciaDisputaService>();
builder.Services.AddScoped<ICalificacionRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.CalificacionRepository>();
builder.Services.AddScoped<ICalificacionService, ProyectoSolucionP2P.CORE.Core.Services.CalificacionService>();
builder.Services.AddScoped<IMensajeRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.MensajeRepository>();
builder.Services.AddScoped<IMensajeService, ProyectoSolucionP2P.CORE.Core.Services.MensajeService>();
builder.Services.AddScoped<INotificacionRepository, ProyectoSolucionP2P.CORE.Infrastructure.Repositories.NotificacionRepository>();
builder.Services.AddScoped<INotificacionService, ProyectoSolucionP2P.CORE.Core.Services.NotificacionService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Deja el MapOpenApi aquí adentro
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Pon a Scalar afuera para asegurarte de que levante sí o sí
app.MapScalarApiReference();

app.UseCors("dev");
app.UseAuthorization();
app.MapControllers();

app.Run();