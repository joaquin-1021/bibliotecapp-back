namespace MiBiblioteca.Application.Commands.Libros;

// Reglas que tiene que cumplir un CrearLibroCommand ANTES de llegar al handler.
// Todas las reglas vienen de LibroDatosValidator (compartidas con ActualizarLibroCommand).
// Se registra solo (ver DependencyInjection.cs) y lo ejecuta ValidationBehavior.
public class CrearLibroCommandValidator : LibroDatosValidator<CrearLibroCommand>;
