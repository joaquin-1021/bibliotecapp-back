using FluentValidation;

namespace MiBiblioteca.Application.Commands.Libros;

// Reglas comunes a crear y actualizar un libro. Es abstracta: no se usa sola,
// sino que CrearLibroCommandValidator y ActualizarLibroCommandValidator heredan de ella.
// "where T : ILibroDatos" permite usarla con cualquier command que tenga los datos de un libro.
public abstract class LibroDatosValidator<T> : AbstractValidator<T> where T : ILibroDatos
{
    protected LibroDatosValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(200).WithMessage("El título no puede superar los 200 caracteres.");

        RuleFor(x => x.Paginas)
            .GreaterThan(0).WithMessage("La cantidad de páginas debe ser mayor a 0.");

        // .When(...) aplica la regla solo si el campo vino cargado (son opcionales)
        RuleFor(x => x.Isbn13)
            .Matches(@"^\d{13}$").WithMessage("El ISBN-13 debe tener exactamente 13 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.Isbn13));

        RuleFor(x => x.Isbn10)
            .Matches(@"^\d{9}[\dXx]$").WithMessage("El ISBN-10 debe tener 10 caracteres: 9 dígitos y un dígito o X al final.")
            .When(x => !string.IsNullOrEmpty(x.Isbn10));

        RuleFor(x => x.AnioPublicacion)
            .Matches(@"^\d{1,4}$").WithMessage("El año de publicación debe ser un número de hasta 4 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.AnioPublicacion));

        RuleFor(x => x.PortadaUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == "http" || uri.Scheme == "https"))
            .WithMessage("La URL de la portada no es válida.")
            .When(x => !string.IsNullOrEmpty(x.PortadaUrl));
    }
}
