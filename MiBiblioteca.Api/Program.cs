using MiBiblioteca.Api.Endpoints;
using MiBiblioteca.Api.Middleware;
using MiBiblioteca.Application;
using MiBiblioteca.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Cada capa registra sus propios servicios
builder.Services.AddApplication();                            // MediatR y handlers
builder.Services.AddInfrastructure(builder.Configuration);    // EF Core + PostgreSQL

// Manejo de errores: convierte excepciones en respuestas ProblemDetails (400, 409, 500)
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorGlobalDeExcepciones>();

// Documento OpenAPI (describe la API en /openapi/v1.json)
builder.Services.AddOpenApi();

var app = builder.Build();

// Va primero: así atrapa las excepciones de todo lo que viene después
app.UseExceptionHandler();

// Si una respuesta de error sale sin cuerpo (ej. JSON mal formado, ruta inexistente),
// le agrega un ProblemDetails para que todos los errores tengan el mismo formato
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoints
app.MapLibrosEndpoints();

app.Run();
