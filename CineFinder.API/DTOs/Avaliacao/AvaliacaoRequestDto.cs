using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Avaliacao;

public class AvaliacaoRequestDto
{
    [Required]
    public int UsuarioId { get; set; }

    public int? FilmeId { get; set; }
    public int? SerieId { get; set; }

    [Required, Range(1, 10)]
    public int Nota { get; set; }

    [MaxLength(1000)]
    public string? Comentario { get; set; }
}
