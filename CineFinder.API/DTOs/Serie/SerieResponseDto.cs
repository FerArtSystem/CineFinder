namespace CineFinder.API.DTOs.Serie;

public class SerieResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Sinopse { get; set; }
    public int? NumTemporadas { get; set; }
    public DateOnly? DataEstreia { get; set; }
    public decimal NotaMedia { get; set; }
    public string? PosterUrl { get; set; }
    public int? GeneroId { get; set; }
    public string? GeneroNome { get; set; }
}
