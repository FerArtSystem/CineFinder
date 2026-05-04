using CineFinder.API.DTOs.Usuario;
using CineFinder.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service) => _service = service;

    /// <summary>Lista todos os usuários.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Busca usuário por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Cadastra um novo usuário.</summary>
    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza dados de um usuário.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] UsuarioRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Remove um usuário.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>Realiza login verificando email e senha.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _service.LoginAsync(dto.Email, dto.Senha);
        return result is null ? Unauthorized(new { mensagem = "Email ou senha inválidos." }) : Ok(result);
    }
}

public record LoginRequestDto(
    [property: System.ComponentModel.DataAnnotations.Required,
     System.ComponentModel.DataAnnotations.EmailAddress] string Email,
    [property: System.ComponentModel.DataAnnotations.Required] string Senha
);
