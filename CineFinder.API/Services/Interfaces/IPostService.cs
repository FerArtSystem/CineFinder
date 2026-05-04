using CineFinder.API.DTOs.Post;

namespace CineFinder.API.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponseDto>> GetAllAsync();
    Task<PostResponseDto?> GetByIdAsync(int id);
    Task<PostResponseDto> CreateAsync(PostRequestDto dto);
    Task<PostResponseDto?> UpdateAsync(int id, PostRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetByUsuarioAsync(int usuarioId);
    Task<IEnumerable<PostResponseDto>> GetRecentesAsync(int quantidade = 20);
}
