using CineFinder.API.DTOs.Favorito;

namespace CineFinder.API.Services.Interfaces;

public interface IFavoritoService
{
    Task<IEnumerable<FavoritoResponseDto>> GetAllAsync();
    Task<FavoritoResponseDto?> GetByIdAsync(int id);
    Task<FavoritoResponseDto> CreateAsync(FavoritoRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FavoritoResponseDto>> GetByUsuarioAsync(int usuarioId);
}
