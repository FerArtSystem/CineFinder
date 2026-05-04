namespace CineFinder.API.DTOs.Post;

public class PostResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNickname { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataEdicao { get; set; }
}
