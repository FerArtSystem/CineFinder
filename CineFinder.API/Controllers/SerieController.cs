using CineFinder.API.DTOs.Serie;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/series")]
public class SerieController : ControllerBase
{
    private readonly ISerieService _service;

    public SerieController(ISerieService service) => _service = service;

    /// <summary>Lista todas as séries com gênero.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SerieResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca série por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SerieResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Busca séries por gênero.</summary>
    [HttpGet("genero/{generoId:int}")]
    public async Task<ActionResult<IEnumerable<SerieResponseDto>>> GetByGenero(int generoId) =>
        Ok(await _service.GetByGeneroAsync(generoId));

    /// <summary>Busca séries por título (busca parcial).</summary>
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<SerieResponseDto>>> Search([FromQuery] string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return BadRequest(new { mensagem = "Parâmetro 'titulo' é obrigatório." });
        return Ok(await _service.SearchByTituloAsync(titulo));
    }

    /// <summary>Cadastra uma nova série.</summary>
    [HttpPost]
    public async Task<ActionResult<SerieResponseDto>> Create([FromBody] SerieRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza uma série.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<SerieResponseDto>> Update(int id, [FromBody] SerieRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove uma série.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
