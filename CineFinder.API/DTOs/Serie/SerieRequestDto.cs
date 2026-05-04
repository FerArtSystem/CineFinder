using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Serie;

public class SerieRequestDto
{
    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Sinopse { get; set; }

    [Range(1, 100)]
    public int? NumTemporadas { get; set; }

    public DateOnly? DataEstreia { get; set; }

    [MaxLength(500)]
    public string? PosterUrl { get; set; }

    public int? GeneroId { get; set; }
}
