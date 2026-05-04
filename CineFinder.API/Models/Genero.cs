using System.ComponentModel.DataAnnotations;

namespace CineFinder.API.Models;

public class Genero
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public ICollection<Filme> Filmes { get; set; } = new List<Filme>();
    public ICollection<Serie> Series { get; set; } = new List<Serie>();
}
