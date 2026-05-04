using CineFinder.API.DTOs.Post;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository) => _repository = repository;

    public async Task<IEnumerable<PostResponseDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToDto);
    }

    public async Task<PostResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : ToDto(item);
    }

    public async Task<PostResponseDto> CreateAsync(PostRequestDto dto)
    {
        var entity = new Post
        {
            UsuarioId = dto.UsuarioId,
            Conteudo = dto.Conteudo,
            DataCriacao = DateTime.UtcNow
        };
        var created = await _repository.CreateAsync(entity);
        var withNav = await _repository.GetByIdAsync(created.Id);
        return ToDto(withNav!);
    }

    public async Task<PostResponseDto?> UpdateAsync(int id, PostRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Conteudo = dto.Conteudo;
        existing.DataEdicao = DateTime.UtcNow;

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

    public async Task<IEnumerable<PostResponseDto>> GetByUsuarioAsync(int usuarioId)
    {
        var items = await _repository.GetByUsuarioAsync(usuarioId);
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<PostResponseDto>> GetRecentesAsync(int quantidade = 20)
    {
        var items = await _repository.GetRecentesAsync(quantidade);
        return items.Select(ToDto);
    }

    private static PostResponseDto ToDto(Post p) => new()
    {
        Id = p.Id,
        UsuarioId = p.UsuarioId,
        UsuarioNickname = p.Usuario?.Nickname ?? string.Empty,
        Conteudo = p.Conteudo,
        DataCriacao = p.DataCriacao,
        DataEdicao = p.DataEdicao
    };
}
