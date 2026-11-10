namespace MiBiblioteca.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TipoUsuarioId { get; set; }
    public DateTime CreatedAt { get; set; }

    //navegación
    public TipoUsuario TipoUsuario { get; set; } = null!;
    public ICollection<Ejemplar> Ejemplares { get; set; } = new List<Ejemplar>();
    public ICollection<Contacto> ContactosAgenda { get; set; } = new List<Contacto>();
}