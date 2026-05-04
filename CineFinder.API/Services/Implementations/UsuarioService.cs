using BCrypt.Net;
using CineFinder.API.DTOs.Usuario;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository) => _repository = repository;

    public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.Select(ToDto);
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        return usuario is null ? null : ToDto(usuario);
    }

    public async Task<UsuarioResponseDto> CreateAsync(UsuarioRequestDto dto)
    {
        var entity = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Nickname = dto.Nickname,
            DataCadastro = DateTime.UtcNow
        };
        var created = await _repository.CreateAsync(entity);
        return ToDto(created);
    }

    public async Task<UsuarioResponseDto?> UpdateAsync(int id, UsuarioRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Nome = dto.Nome;
        existing.Email = dto.Email;
        existing.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
        existing.Nickname = dto.Nickname;

        var updated = await _repository.UpdateAsync(existing);
        return ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;
        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<UsuarioResponseDto?> LoginAsync(string email, string senha)
    {
        var usuario = await _repository.GetByEmailAsync(email);
        if (usuario is null) return null;
        if (!BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash)) return null;
        return ToDto(usuario);
    }

    private static UsuarioResponseDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Nome = u.Nome,
        Email = u.Email,
        Nickname = u.Nickname,
        DataCadastro = u.DataCadastro
    };
}
