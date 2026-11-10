namespace MiBiblioteca.Application.Exceptions;

// Se lanza cuando el pedido es válido pero choca con datos que ya existen
// (por ejemplo, un ISBN repetido). La Api la traduce a 409 Conflict.
public class ConflictoException(string mensaje) : Exception(mensaje);
