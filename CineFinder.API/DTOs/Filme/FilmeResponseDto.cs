namespace CineFinder.API.DTOs.Filme;

public class FilmeResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Sinopse { get; set; }
    public DateOnly? DataLancamento { get; set; }
    public int? DuracaoMinutos { get; set; }
    public decimal NotaMedia { get; set; }
    public string? PosterUrl { get; set; }
    public int? GeneroId { get; set; }
    public string? GeneroNome { get; set; }
}
