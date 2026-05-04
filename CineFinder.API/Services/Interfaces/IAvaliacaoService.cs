using CineFinder.API.DTOs.Avaliacao;

namespace CineFinder.API.Services.Interfaces;

public interface IAvaliacaoService
{
    Task<IEnumerable<AvaliacaoResponseDto>> GetAllAsync();
    Task<AvaliacaoResponseDto?> GetByIdAsync(int id);
    Task<AvaliacaoResponseDto> CreateAsync(AvaliacaoRequestDto dto);
    Task<AvaliacaoResponseDto?> UpdateAsync(int id, AvaliacaoRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AvaliacaoResponseDto>> GetByFilmeAsync(int filmeId);
    Task<IEnumerable<AvaliacaoResponseDto>> GetBySerieAsync(int serieId);
    Task<IEnumerable<AvaliacaoResponseDto>> GetByUsuarioAsync(int usuarioId);
}
