using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Usuario;

public class UsuarioRequestDto
{
    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(128)]
    public string Senha { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;
}
