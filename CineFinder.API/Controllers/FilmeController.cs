using CineFinder.API.DTOs.Filme;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/filmes")]
public class FilmeController : ControllerBase
{
    private readonly IFilmeService _service;

    public FilmeController(IFilmeService service) => _service = service;

    /// <summary>Lista todos os filmes com gênero.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FilmeResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca filme por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FilmeResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Busca filmes por gênero.</summary>
    [HttpGet("genero/{generoId:int}")]
    public async Task<ActionResult<IEnumerable<FilmeResponseDto>>> GetByGenero(int generoId) =>
        Ok(await _service.GetByGeneroAsync(generoId));

    /// <summary>Busca filmes por título (busca parcial).</summary>
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<FilmeResponseDto>>> Search([FromQuery] string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return BadRequest(new { mensagem = "Parâmetro 'titulo' é obrigatório." });
        return Ok(await _service.SearchByTituloAsync(titulo));
    }

    /// <summary>Cadastra um novo filme.</summary>
    [HttpPost]
    public async Task<ActionResult<FilmeResponseDto>> Create([FromBody] FilmeRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um filme.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<FilmeResponseDto>> Update(int id, [FromBody] FilmeRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove um filme.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
