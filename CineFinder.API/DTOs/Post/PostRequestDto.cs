using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Post;

public class PostRequestDto
{
    [Required]
    public int UsuarioId { get; set; }

    [Required, MinLength(1), MaxLength(2000)]
    public string Conteudo { get; set; } = string.Empty;
}
