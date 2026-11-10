namespace MiBiblioteca.Application.Exceptions;

// Se lanza cuando se pide algo que no existe (por ejemplo, un libro con un id inexistente).
// La Api la traduce a 404 Not Found.
public class NoEncontradoException(string mensaje) : Exception(mensaje);
