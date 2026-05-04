using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineFinder.API.Models;

public class Serie
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Sinopse { get; set; }

    [Range(1, 100)]
    public int? NumTemporadas { get; set; }

    public DateOnly? DataEstreia { get; set; }

    [Range(0.0, 10.0)]
    [Column(TypeName = "decimal(4,2)")]
    public decimal NotaMedia { get; set; } = 0;

    [MaxLength(500)]
    public string? PosterUrl { get; set; }

    public int? GeneroId { get; set; }

    [ForeignKey(nameof(GeneroId))]
    public Genero? Genero { get; set; }

    public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
}
