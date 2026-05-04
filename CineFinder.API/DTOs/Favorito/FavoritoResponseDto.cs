namespace CineFinder.API.DTOs.Favorito;

public class FavoritoResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNickname { get; set; } = string.Empty;
    public int? FilmeId { get; set; }
    public string? FilmeTitulo { get; set; }
    public int? SerieId { get; set; }
    public string? SerieTitulo { get; set; }
    public DateTime DataAdicionado { get; set; }
}
