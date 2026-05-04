using CineFinder.API.DTOs.Filme;

namespace CineFinder.API.Services.Interfaces;

public interface IFilmeService
{
    Task<IEnumerable<FilmeResponseDto>> GetAllAsync();
    Task<FilmeResponseDto?> GetByIdAsync(int id);
    Task<FilmeResponseDto> CreateAsync(FilmeRequestDto dto);
    Task<FilmeResponseDto?> UpdateAsync(int id, FilmeRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FilmeResponseDto>> GetByGeneroAsync(int generoId);
    Task<IEnumerable<FilmeResponseDto>> SearchByTituloAsync(string titulo);
}
