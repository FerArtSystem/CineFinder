using CineFinder.API.DTOs.Genero;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/generos")]
public class GeneroController : ControllerBase
{
    private readonly IGeneroService _service;

    public GeneroController(IGeneroService service) => _service = service;

    /// <summary>Lista todos os gêneros.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GeneroResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca gênero por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GeneroResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Cria um novo gênero.</summary>
    [HttpPost]
    public async Task<ActionResult<GeneroResponseDto>> Create([FromBody] GeneroRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um gênero.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<GeneroResponseDto>> Update(int id, [FromBody] GeneroRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove um gênero.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
