using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineFinder.API.Models;

public class Avaliacao
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }

    public int? FilmeId { get; set; }

    [ForeignKey(nameof(FilmeId))]
    public Filme? Filme { get; set; }

    public int? SerieId { get; set; }

    [ForeignKey(nameof(SerieId))]
    public Serie? Serie { get; set; }

    [Required, Range(1, 10)]
    public int Nota { get; set; }

    [MaxLength(1000)]
    public string? Comentario { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
