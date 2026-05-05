using System.Net.Http.Json;
using System.Text.Json;
using CineFinder.API.DTOs.Avaliacao;
using CineFinder.API.DTOs.Favorito;
using CineFinder.API.DTOs.Post;
using CineFinder.API.DTOs.Usuario;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

/// <summary>
/// Implementação que consome a API Java (Spring Boot, porta 8080) via HttpClient.
/// Registrado como typed client em Program.cs.
/// </summary>
public class JavaApiService : IJavaApiService
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JavaApiService(HttpClient http) => _http = http;

    // ─────────────────────────────────────────────────────────────────
    // Helpers genéricos
    // ─────────────────────────────────────────────────────────────────

    private async Task<T?> GetAsync<T>(string path)
    {
        try
        {
            return await _http.GetFromJsonAsync<T>(path, _jsonOpts);
        }
        catch
        {
            return default;
        }
    }

    private async Task<T?> PostAsync<T>(string path, object body)
    {
        var response = await _http.PostAsJsonAsync(path, body);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(errorBody, null, response.StatusCode);
        }
        return await response.Content.ReadFromJsonAsync<T>(_jsonOpts);
    }

    private async Task<T?> PutAsync<T>(string path, object body)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(path, body);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOpts);
        }
        catch
        {
            return default;
        }
    }

    private async Task<bool> DeleteAsync(string path)
    {
        try
        {
            var response = await _http.DeleteAsync(path);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // ─────────────────────────────────────────────────────────────────
    // Usuários
    // ─────────────────────────────────────────────────────────────────

    public Task<IEnumerable<UsuarioResponseDto>> GetUsuariosAsync()
        => GetAsync<IEnumerable<UsuarioResponseDto>>("api/usuarios")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<UsuarioResponseDto>());

    public Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id)
        => GetAsync<UsuarioResponseDto>($"api/usuarios/{id}");

    public Task<UsuarioResponseDto?> CreateUsuarioAsync(object body)
        => PostAsync<UsuarioResponseDto>("api/usuarios", body);

    public Task<UsuarioResponseDto?> UpdateUsuarioAsync(int id, object body)
        => PutAsync<UsuarioResponseDto>($"api/usuarios/{id}", body);

    public Task<bool> DeleteUsuarioAsync(int id)
        => DeleteAsync($"api/usuarios/{id}");

    public Task<UsuarioResponseDto?> LoginAsync(object body)
        => PostAsync<UsuarioResponseDto>("api/usuarios/login", body);

    // ─────────────────────────────────────────────────────────────────
    // Avaliações
    // ─────────────────────────────────────────────────────────────────

    public Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesAsync()
        => GetAsync<IEnumerable<AvaliacaoResponseDto>>("api/avaliacoes")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<AvaliacaoResponseDto>());

    public Task<AvaliacaoResponseDto?> GetAvaliacaoByIdAsync(int id)
        => GetAsync<AvaliacaoResponseDto>($"api/avaliacoes/{id}");

    public Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesByFilmeAsync(int filmeId)
        => GetAsync<IEnumerable<AvaliacaoResponseDto>>($"api/avaliacoes/filme/{filmeId}")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<AvaliacaoResponseDto>());

    public Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesBySerieAsync(int serieId)
        => GetAsync<IEnumerable<AvaliacaoResponseDto>>($"api/avaliacoes/serie/{serieId}")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<AvaliacaoResponseDto>());

    public Task<IEnumerable<AvaliacaoResponseDto>> GetAvaliacoesByUsuarioAsync(int usuarioId)
        => GetAsync<IEnumerable<AvaliacaoResponseDto>>($"api/avaliacoes/usuario/{usuarioId}")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<AvaliacaoResponseDto>());

    public Task<AvaliacaoResponseDto?> CreateAvaliacaoAsync(object body)
        => PostAsync<AvaliacaoResponseDto>("api/avaliacoes", body);

    public Task<AvaliacaoResponseDto?> UpdateAvaliacaoAsync(int id, object body)
        => PutAsync<AvaliacaoResponseDto>($"api/avaliacoes/{id}", body);

    public Task<bool> DeleteAvaliacaoAsync(int id)
        => DeleteAsync($"api/avaliacoes/{id}");

    // ─────────────────────────────────────────────────────────────────
    // Favoritos
    // ─────────────────────────────────────────────────────────────────

    public Task<IEnumerable<FavoritoResponseDto>> GetFavoritosAsync()
        => GetAsync<IEnumerable<FavoritoResponseDto>>("api/favoritos")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<FavoritoResponseDto>());

    public Task<FavoritoResponseDto?> GetFavoritoByIdAsync(int id)
        => GetAsync<FavoritoResponseDto>($"api/favoritos/{id}");

    public Task<IEnumerable<FavoritoResponseDto>> GetFavoritosByUsuarioAsync(int usuarioId)
        => GetAsync<IEnumerable<FavoritoResponseDto>>($"api/favoritos/usuario/{usuarioId}")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<FavoritoResponseDto>());

    public Task<FavoritoResponseDto?> CreateFavoritoAsync(object body)
        => PostAsync<FavoritoResponseDto>("api/favoritos", body);

    public Task<bool> DeleteFavoritoAsync(int id)
        => DeleteAsync($"api/favoritos/{id}");

    // ─────────────────────────────────────────────────────────────────
    // Posts
    // ─────────────────────────────────────────────────────────────────

    public Task<IEnumerable<PostResponseDto>> GetPostsAsync()
        => GetAsync<IEnumerable<PostResponseDto>>("api/posts")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<PostResponseDto>());

    public Task<PostResponseDto?> GetPostByIdAsync(int id)
        => GetAsync<PostResponseDto>($"api/posts/{id}");

    public Task<IEnumerable<PostResponseDto>> GetPostsByUsuarioAsync(int usuarioId)
        => GetAsync<IEnumerable<PostResponseDto>>($"api/posts/usuario/{usuarioId}")!
           .ContinueWith(t => t.Result ?? Enumerable.Empty<PostResponseDto>());

    public Task<PostResponseDto?> CreatePostAsync(object body)
        => PostAsync<PostResponseDto>("api/posts", body);

    public Task<PostResponseDto?> UpdatePostAsync(int id, object body)
        => PutAsync<PostResponseDto>($"api/posts/{id}", body);

    public Task<bool> DeletePostAsync(int id)
        => DeleteAsync($"api/posts/{id}");

    // ─────────────────────────────────────────────────────────────────
    // Cinemas (agregador – sem entidade JPA na Java API)
    // ─────────────────────────────────────────────────────────────────

    public Task<object?> GetCinemasProximosAsync(double lat, double lng, double raio)
        => GetAsync<object>($"api/cinemas/proximos?lat={lat}&lng={lng}&raio={raio}");

    public Task<object?> GetCinemasByCidadeAsync(string cidade, double raio)
        => GetAsync<object>($"api/cinemas/cidade?q={Uri.EscapeDataString(cidade)}&raio={raio}");
}
