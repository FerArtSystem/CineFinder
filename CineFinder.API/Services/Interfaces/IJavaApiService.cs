using CineFinder.API.DTOs.Avaliacao;
using CineFinder.API.DTOs.Favorito;
using CineFinder.API.DTOs.Post;
using CineFinder.API.DTOs.Usuario;

namespace CineFinder.API.Services.Interfaces;

/// <summary>
/// Contrato para consumo da API Java (social + cinemas).
/// Todos os métodos delegam chamadas HTTP à instância em http://localhost:8080.
/// </summary>
public interface IJavaApiService
{
    // ── Usuários ────────────────────────────────────────────────────
    Task<IEnumerable<UsuarioResponseDto>> GetUsuariosAsync();
    Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id);
    Task<UsuarioResponseDto?> CreateUsuarioAsync(object body);
    Task<UsuarioResponseDto?> UpdateUsuarioAsync(int id, object body);
    Task<bool> DeleteUsuarioAsync(int id);
    Task<UsuarioResponseDto?> LoginAsync(object body);

    // ── Avaliações ──────────────────────────────────────────────────
    Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesAsync();
    Task<AvaliacaoResponseDto?> GetAvaliacaoByIdAsync(int id);
    Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesByFilmeAsync(int filmeId);
    Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesBySerieAsync(int serieId);
    Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesByUsuarioAsync(int usuarioId);
    Task<AvaliacaoResponseDto?> CreateAvaliacaoAsync(object body);
    Task<AvaliacaoResponseDto?> UpdateAvaliacaoAsync(int id, object body);
    Task<bool> DeleteAvaliacaoAsync(int id);

    // ── Favoritos ───────────────────────────────────────────────────
    Task<IEnumerable<FavoritoResponseDto>> GetFavoritosAsync();
    Task<FavoritoResponseDto?> GetFavoritoByIdAsync(int id);
    Task<IEnumerable<FavoritoResponseDto>> GetFavoritosByUsuarioAsync(int usuarioId);
    Task<FavoritoResponseDto?> CreateFavoritoAsync(object body);
    Task<bool> DeleteFavoritoAsync(int id);

    // ── Posts ───────────────────────────────────────────────────────
    Task<IEnumerable<PostResponseDto>> GetPostsAsync();
    Task<PostResponseDto?> GetPostByIdAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetPostsByUsuarioAsync(int usuarioId);
    Task<PostResponseDto?> CreatePostAsync(object body);
    Task<PostResponseDto?> UpdatePostAsync(int id, object body);
    Task<bool> DeletePostAsync(int id);

    // ── Cinemas (agregador OpenStreetMap) ───────────────────────────
    Task<object?> GetCinemasProximosAsync(double lat, double lng, double raio);
    Task<object?> GetCinemasByCidadeAsync(string cidade, double raio);
}
