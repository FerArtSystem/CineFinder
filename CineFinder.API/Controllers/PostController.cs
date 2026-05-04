using CineFinder.API.DTOs.Post;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _service;

    public PostController(IPostService service) => _service = service;

    /// <summary>Lista todos os posts.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca post por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Lista posts de um usuário.</summary>
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetByUsuario(int usuarioId) =>
        Ok(await _service.GetByUsuarioAsync(usuarioId));

    /// <summary>Lista posts mais recentes (padrão: 20).</summary>
    [HttpGet("recentes")]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetRecentes([FromQuery] int quantidade = 20) =>
        Ok(await _service.GetRecentesAsync(quantidade));

    /// <summary>Cria um post na comunidade.</summary>
    [HttpPost]
    public async Task<ActionResult<PostResponseDto>> Create([FromBody] PostRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Edita conteúdo de um post.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostResponseDto>> Update(int id, [FromBody] PostRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove um post.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
