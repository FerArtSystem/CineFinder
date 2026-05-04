namespace CineFinder.API.DTOs.Avaliacao;

public class AvaliacaoResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNickname { get; set; } = string.Empty;
    public int? FilmeId { get; set; }
    public string? FilmeTitulo { get; set; }
    public int? SerieId { get; set; }
    public string? SerieTitulo { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime DataCriacao { get; set; }
}
