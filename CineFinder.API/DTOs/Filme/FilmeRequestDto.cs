using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Filme;

public class FilmeRequestDto
{
    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Sinopse { get; set; }

    public DateOnly? DataLancamento { get; set; }

    [Range(1, 600)]
    public int? DuracaoMinutos { get; set; }

    [MaxLength(500)]
    public string? PosterUrl { get; set; }

    public int? GeneroId { get; set; }
}
