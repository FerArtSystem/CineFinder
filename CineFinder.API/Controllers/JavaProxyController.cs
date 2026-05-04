using CineFinder.API.DTOs.Avaliacao;
using CineFinder.API.DTOs.Favorito;
using CineFinder.API.DTOs.Post;
using CineFinder.API.DTOs.Usuario;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

/// <summary>
/// Proxy transparente para a API Java (Spring Boot, porta 8080).
/// O C# recebe as requisições, chama a Java API internamente via HttpClient
/// e retorna a resposta ao cliente — sem expor a porta 8080 diretamente.
///
/// Base: /api/java
/// </summary>
[ApiController]
[Route("api/java")]
public class JavaProxyController : ControllerBase
{
    private readonly IJavaApiService _java;

    public JavaProxyController(IJavaApiService java) => _java = java;

    // ────────────────────────────────────────────────────────────────
    // Usuários
    // ────────────────────────────────────────────────────────────────

    /// <summary>Lista todos os usuários (via Java API).</summary>
    [HttpGet("usuarios")]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        => Ok(await _java.GetUsuariosAsync());

    /// <summary>Busca usuário por ID (via Java API).</summary>
    [HttpGet("usuarios/{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(int id)
    {
        var result = await _java.GetUsuarioByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Cria usuário (via Java API).</summary>
    [HttpPost("usuarios")]
    public async Task<ActionResult<UsuarioResponseDto>> CreateUsuario([FromBody] object body)
    {
        var result = await _java.CreateUsuarioAsync(body);
        return result is null ? BadRequest(new { message = "Erro ao criar usuário na API Java." }) : Ok(result);
    }

    /// <summary>Atualiza usuário (via Java API).</summary>
    [HttpPut("usuarios/{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> UpdateUsuario(int id, [FromBody] object body)
    {
        var result = await _java.UpdateUsuarioAsync(id, body);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove usuário (via Java API).</summary>
    [HttpDelete("usuarios/{id:int}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var ok = await _java.DeleteUsuarioAsync(id);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Login de usuário (via Java API).</summary>
    [HttpPost("usuarios/login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] object body)
    {
        var result = await _java.LoginAsync(body);
        return result is null
            ? Unauthorized(new { message = "Credenciais inválidas (Java API)." })
            : Ok(result);
    }

    // ────────────────────────────────────────────────────────────────
    // Avaliações
    // ────────────────────────────────────────────────────────────────

    /// <summary>Lista todas as avaliações (via Java API).</summary>
    [HttpGet("avaliacoes")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetAvaliacoes()
        => Ok(await _java.GetAvaliacoesAsync());

    /// <summary>Busca avaliação por ID (via Java API).</summary>
    [HttpGet("avaliacoes/{id:int}")]
    public async Task<ActionResult<AvaliacaoResponseDto>> GetAvaliacao(int id)
    {
        var result = await _java.GetAvaliacaoByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Avaliações de um filme (via Java API).</summary>
    [HttpGet("avaliacoes/filme/{filmeId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetAvaliacoesByFilme(int filmeId)
        => Ok(await _java.GetAvaliacoesByFilmeAsync(filmeId));

    /// <summary>Avaliações de uma série (via Java API).</summary>
    [HttpGet("avaliacoes/serie/{serieId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetAvaliacoesBySerie(int serieId)
        => Ok(await _java.GetAvaliacoesBySerieAsync(serieId));

    /// <summary>Avaliações de um usuário (via Java API).</summary>
    [HttpGet("avaliacoes/usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetAvaliacoesByUsuario(int usuarioId)
        => Ok(await _java.GetAvaliacoesByUsuarioAsync(usuarioId));

    /// <summary>Cria avaliação (via Java API).</summary>
    [HttpPost("avaliacoes")]
    public async Task<ActionResult<AvaliacaoResponseDto>> CreateAvaliacao([FromBody] object body)
    {
        var result = await _java.CreateAvaliacaoAsync(body);
        return result is null ? BadRequest(new { message = "Erro ao criar avaliação na API Java." }) : Ok(result);
    }

    /// <summary>Atualiza avaliação (via Java API).</summary>
    [HttpPut("avaliacoes/{id:int}")]
    public async Task<ActionResult<AvaliacaoResponseDto>> UpdateAvaliacao(int id, [FromBody] object body)
    {
        var result = await _java.UpdateAvaliacaoAsync(id, body);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove avaliação (via Java API).</summary>
    [HttpDelete("avaliacoes/{id:int}")]
    public async Task<IActionResult> DeleteAvaliacao(int id)
    {
        var ok = await _java.DeleteAvaliacaoAsync(id);
        return ok ? NoContent() : NotFound();
    }

    // ────────────────────────────────────────────────────────────────
    // Favoritos
    // ────────────────────────────────────────────────────────────────

    /// <summary>Lista todos os favoritos (via Java API).</summary>
    [HttpGet("favoritos")]
    public async Task<ActionResult<IEnumerable<FavoritoResponseDto>>> GetFavoritos()
        => Ok(await _java.GetFavoritosAsync());

    /// <summary>Busca favorito por ID (via Java API).</summary>
    [HttpGet("favoritos/{id:int}")]
    public async Task<ActionResult<FavoritoResponseDto>> GetFavorito(int id)
    {
        var result = await _java.GetFavoritoByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Favoritos de um usuário (via Java API).</summary>
    [HttpGet("favoritos/usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<FavoritoResponseDto>>> GetFavoritosByUsuario(int usuarioId)
        => Ok(await _java.GetFavoritosByUsuarioAsync(usuarioId));

    /// <summary>Adiciona favorito (via Java API).</summary>
    [HttpPost("favoritos")]
    public async Task<ActionResult<FavoritoResponseDto>> CreateFavorito([FromBody] object body)
    {
        var result = await _java.CreateFavoritoAsync(body);
        return result is null ? BadRequest(new { message = "Erro ao adicionar favorito na API Java." }) : Ok(result);
    }

    /// <summary>Remove favorito (via Java API).</summary>
    [HttpDelete("favoritos/{id:int}")]
    public async Task<IActionResult> DeleteFavorito(int id)
    {
        var ok = await _java.DeleteFavoritoAsync(id);
        return ok ? NoContent() : NotFound();
    }

    // ────────────────────────────────────────────────────────────────
    // Posts
    // ────────────────────────────────────────────────────────────────

    /// <summary>Feed de posts (via Java API).</summary>
    [HttpGet("posts")]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPosts()
        => Ok(await _java.GetPostsAsync());

    /// <summary>Busca post por ID (via Java API).</summary>
    [HttpGet("posts/{id:int}")]
    public async Task<ActionResult<PostResponseDto>> GetPost(int id)
    {
        var result = await _java.GetPostByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Posts de um usuário (via Java API).</summary>
    [HttpGet("posts/usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPostsByUsuario(int usuarioId)
        => Ok(await _java.GetPostsByUsuarioAsync(usuarioId));

    /// <summary>Cria post (via Java API).</summary>
    [HttpPost("posts")]
    public async Task<ActionResult<PostResponseDto>> CreatePost([FromBody] object body)
    {
        var result = await _java.CreatePostAsync(body);
        return result is null ? BadRequest(new { message = "Erro ao criar post na API Java." }) : Ok(result);
    }

    /// <summary>Atualiza post (via Java API).</summary>
    [HttpPut("posts/{id:int}")]
    public async Task<ActionResult<PostResponseDto>> UpdatePost(int id, [FromBody] object body)
    {
        var result = await _java.UpdatePostAsync(id, body);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove post (via Java API).</summary>
    [HttpDelete("posts/{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var ok = await _java.DeletePostAsync(id);
        return ok ? NoContent() : NotFound();
    }

    // ────────────────────────────────────────────────────────────────
    // Cinemas (agregador OpenStreetMap via Java API)
    // ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Cinemas próximos por coordenadas GPS — C# chama Java que chama Overpass API.
    /// GET /api/java/cinemas/proximos?lat=-23.55&lng=-46.63&raio=5
    /// </summary>
    [HttpGet("cinemas/proximos")]
    public async Task<ActionResult<object>> GetCinemasProximos(
        [FromQuery] double lat,
        [FromQuery] double lng,
        [FromQuery] double raio = 5)
    {
        var result = await _java.GetCinemasProximosAsync(lat, lng, raio);
        return result is null
            ? StatusCode(503, new { message = "API Java indisponível para busca de cinemas." })
            : Ok(result);
    }

    /// <summary>
    /// Cinemas por nome de cidade — C# chama Java que geocodifica e busca no Overpass.
    /// GET /api/java/cinemas/cidade?q=São Paulo&raio=5
    /// </summary>
    [HttpGet("cinemas/cidade")]
    public async Task<ActionResult<object>> GetCinemasByCidade(
        [FromQuery] string q,
        [FromQuery] double raio = 5)
    {
        var result = await _java.GetCinemasByCidadeAsync(q, raio);
        return result is null
            ? StatusCode(503, new { message = "API Java indisponível para busca de cinemas por cidade." })
            : Ok(result);
    }
}
