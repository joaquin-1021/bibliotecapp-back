using FluentValidation;

namespace MiBiblioteca.Application.Commands.Libros;

// Hereda las reglas comunes y agrega la propia: el id tiene que ser válido.
public class ActualizarLibroCommandValidator : LibroDatosValidator<ActualizarLibroCommand>
{
    public ActualizarLibroCommandValidator()
    {
        RuleFor(x => x.LibroId)
            .GreaterThan(0).WithMessage("El id del libro debe ser mayor a 0.");
    }
}
