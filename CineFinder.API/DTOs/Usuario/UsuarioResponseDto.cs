namespace CineFinder.API.DTOs.Usuario;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}
