using CineFinder.API.DTOs.Usuario;

namespace CineFinder.API.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(int id);
    Task<UsuarioResponseDto> CreateAsync(UsuarioRequestDto dto);
    Task<UsuarioResponseDto?> UpdateAsync(int id, UsuarioRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<UsuarioResponseDto?> LoginAsync(string email, string senha);
}
