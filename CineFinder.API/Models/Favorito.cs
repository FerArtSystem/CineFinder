using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineFinder.API.Models;

public class Favorito
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }

    public int? FilmeId { get; set; }

    [ForeignKey(nameof(FilmeId))]
    public Filme? Filme { get; set; }

    public int? SerieId { get; set; }

    [ForeignKey(nameof(SerieId))]
    public Serie? Serie { get; set; }

    public DateTime DataAdicionado { get; set; } = DateTime.UtcNow;
}
