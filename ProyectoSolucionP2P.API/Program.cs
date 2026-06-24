using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoSolucionP2P.CORE.Core.Interfaces;
using ProyectoSolucionP2P.CORE.Core.Services;
using ProyectoSolucionP2P.CORE.Core.Settings;
using ProyectoSolucionP2P.CORE.Infrastructure.Data;
using ProyectoSolucionP2P.CORE.Infrastructure.Repositories;
using ProyectoSolucionP2P.CORE.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// --- Base de datos ---
var cnx = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<CambioSeguroP2pdbContext>(o => o.UseSqlServer(cnx));

// --- JWT ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtService, JwtService>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// --- CORS ---
builder.Services.AddCors(o => o.AddPolicy("dev",
    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// --- Repositories ---
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMonedaRepository, MonedaRepository>();
builder.Services.AddScoped<IMetodoPagoRepository, MetodoPagoRepository>();
builder.Services.AddScoped<IVerificacionIdentidadRepository, VerificacionIdentidadRepository>();
builder.Services.AddScoped<IReporteAdministrativoRepository, ReporteAdministrativoRepository>();
builder.Services.AddScoped<IOfertaRepository, OfertaRepository>();
builder.Services.AddScoped<IOfertaMetodoPagoRepository, OfertaMetodoPagoRepository>();
builder.Services.AddScoped<IOperacionRepository, OperacionRepository>();
builder.Services.AddScoped<ITemporizadorOperacionRepository, TemporizadorOperacionRepository>();
builder.Services.AddScoped<IComprobantePagoRepository, ComprobantePagoRepository>();
builder.Services.AddScoped<IDisputaRepository, DisputaRepository>();
builder.Services.AddScoped<IEvidenciaDisputaRepository, EvidenciaDisputaRepository>();
builder.Services.AddScoped<ICalificacionRepository, CalificacionRepository>();
builder.Services.AddScoped<IMensajeRepository, MensajeRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();

// --- Services ---
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMonedaService, MonedaService>();
builder.Services.AddScoped<IMetodoPagoService, MetodoPagoService>();
builder.Services.AddScoped<IVerificacionIdentidadService, VerificacionIdentidadService>();
builder.Services.AddScoped<IReporteAdministrativoService, ReporteAdministrativoService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();
builder.Services.AddScoped<IOfertaMetodoPagoService, OfertaMetodoPagoService>();
builder.Services.AddScoped<IOperacionService, OperacionService>();
builder.Services.AddScoped<ITemporizadorOperacionService, TemporizadorOperacionService>();
builder.Services.AddScoped<IComprobantePagoService, ComprobantePagoService>();
builder.Services.AddScoped<IDisputaService, DisputaService>();
builder.Services.AddScoped<IEvidenciaDisputaService, EvidenciaDisputaService>();
builder.Services.AddScoped<ICalificacionService, CalificacionService>();
builder.Services.AddScoped<IMensajeService, MensajeService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors("dev");
app.UseAuthentication();   // primero: ¿quién eres?
app.UseAuthorization();    // luego: ¿puedes entrar?
app.MapControllers();

app.Run();