using CineFinder.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Serie> Series { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Email único por usuário
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Nickname único
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Nickname)
            .IsUnique();

        // Um Favorito referencia filme OU série (não ambos)
        modelBuilder.Entity<Favorito>().ToTable(t =>
            t.HasCheckConstraint("CK_Favorito_FilmeOuSerie",
                "(FilmeId IS NOT NULL AND SerieId IS NULL) OR (FilmeId IS NULL AND SerieId IS NOT NULL)"));

        // Uma Avaliação referencia filme OU série (não ambos)
        modelBuilder.Entity<Avaliacao>().ToTable(t =>
            t.HasCheckConstraint("CK_Avaliacao_FilmeOuSerie",
                "(FilmeId IS NOT NULL AND SerieId IS NULL) OR (FilmeId IS NULL AND SerieId IS NOT NULL)"));
    }
}
