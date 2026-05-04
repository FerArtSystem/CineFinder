using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Genero;

public class GeneroRequestDto
{
    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
}
