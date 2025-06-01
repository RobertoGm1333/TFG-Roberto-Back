using ProtectoraAPI.Controllers;
using ProtectoraAPI.Repositories;
using ProtectoraAPI.Services;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GatosDB");

// Repositorios
builder.Services.AddScoped<IGatoRepository, GatoRepository>(provider =>
    new GatoRepository(connectionString));

builder.Services.AddScoped<IProtectoraRepository, ProtectoraRepository>(provider =>
    new ProtectoraRepository(connectionString));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));

builder.Services.AddScoped<IDeseadoRepository, DeseadoRepository>(provider =>
    new DeseadoRepository(connectionString));

builder.Services.AddScoped<ISolicitudAdopcionRepository, SolicitudAdopcionRepository>(provider =>
    new SolicitudAdopcionRepository(connectionString));

// Servicios
builder.Services.AddScoped<IGatoService, GatoService>(provider =>
    new GatoService(provider.GetRequiredService<IGatoRepository>()));

builder.Services.AddScoped<IProtectoraService, ProtectoraService>(provider =>
    new ProtectoraService(provider.GetRequiredService<IProtectoraRepository>()));

builder.Services.AddScoped<IUsuarioService, UsuarioService>(provider =>
    new UsuarioService(provider.GetRequiredService<IUsuarioRepository>()));

builder.Services.AddScoped<IDeseadoService, DeseadoService>(provider =>
    new DeseadoService(provider.GetRequiredService<IDeseadoRepository>()));

builder.Services.AddScoped<ISolicitudAdopcionService, SolicitudAdopcionService>(provider =>
    new SolicitudAdopcionService(provider.GetRequiredService<ISolicitudAdopcionRepository>()));

var AllowAll = "_AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Agregar controladores con opciones para manejar referencias circulares
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configuración Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Configurar CORS
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();

// Configurar el servicio de archivos estáticos
app.UseStaticFiles(); // Para wwwroot

// Configurar el servicio de archivos estáticos para las imágenes del formulario
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images", "form")),
    RequestPath = "/images"
});

app.MapControllers();

app.Run();
