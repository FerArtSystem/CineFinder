using CineFinder.API.DTOs.Serie;

namespace CineFinder.API.Services.Interfaces;

public interface ISerieService
{
    Task<IEnumerable<SerieResponseDto>> GetAllAsync();
    Task<SerieResponseDto?> GetByIdAsync(int id);
    Task<SerieResponseDto> CreateAsync(SerieRequestDto dto);
    Task<SerieResponseDto?> UpdateAsync(int id, SerieRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SerieResponseDto>> GetByGeneroAsync(int generoId);
    Task<IEnumerable<SerieResponseDto>> SearchByTituloAsync(string titulo);
}
