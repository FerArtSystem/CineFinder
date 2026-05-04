using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.DTOs.Favorito;

public class FavoritoRequestDto
{
    [Required]
    public int UsuarioId { get; set; }

    public int? FilmeId { get; set; }
    public int? SerieId { get; set; }
}
