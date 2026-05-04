using CineFinder.API.DTOs.Avaliacao;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/avaliacoes")]
public class AvaliacaoController : ControllerBase
{
    private readonly IAvaliacaoService _service;

    public AvaliacaoController(IAvaliacaoService service) => _service = service;

    /// <summary>Lista todas as avaliações.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca avaliação por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AvaliacaoResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Lista avaliações de um filme.</summary>
    [HttpGet("filme/{filmeId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetByFilme(int filmeId) =>
        Ok(await _service.GetByFilmeAsync(filmeId));

    /// <summary>Lista avaliações de uma série.</summary>
    [HttpGet("serie/{serieId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetBySerie(int serieId) =>
        Ok(await _service.GetBySerieAsync(serieId));

    /// <summary>Lista avaliações de um usuário.</summary>
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponseDto>>> GetByUsuario(int usuarioId) =>
        Ok(await _service.GetByUsuarioAsync(usuarioId));

    /// <summary>Cria uma avaliação.</summary>
    [HttpPost]
    public async Task<ActionResult<AvaliacaoResponseDto>> Create([FromBody] AvaliacaoRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza nota/comentário de uma avaliação.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AvaliacaoResponseDto>> Update(int id, [FromBody] AvaliacaoRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove uma avaliação.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
