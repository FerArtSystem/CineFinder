using CineFinder.API.DTOs.Genero;

namespace CineFinder.API.Services.Interfaces;

public interface IGeneroService
{
    Task<IEnumerable<GeneroResponseDto>> GetAllAsync();
    Task<GeneroResponseDto?> GetByIdAsync(int id);
    Task<GeneroResponseDto> CreateAsync(GeneroRequestDto dto);
    Task<GeneroResponseDto?> UpdateAsync(int id, GeneroRequestDto dto);
    Task<bool> DeleteAsync(int id);
}
