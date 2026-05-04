using CineFinder.API.DTOs.Favorito;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/favoritos")]
public class FavoritoController : ControllerBase
{
    private readonly IFavoritoService _service;

    public FavoritoController(IFavoritoService service) => _service = service;

    /// <summary>Lista todos os favoritos.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoritoResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca favorito por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FavoritoResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Lista favoritos de um usuário.</summary>
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<ActionResult<IEnumerable<FavoritoResponseDto>>> GetByUsuario(int usuarioId) =>
        Ok(await _service.GetByUsuarioAsync(usuarioId));

    /// <summary>Adiciona um favorito.</summary>
    [HttpPost]
    public async Task<ActionResult<FavoritoResponseDto>> Create([FromBody] FavoritoRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Remove um favorito.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
