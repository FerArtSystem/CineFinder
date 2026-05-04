using CineFinder.API.DTOs.Serie;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class SerieService : ISerieService
{
    private readonly ISerieRepository _repository;

    public SerieService(ISerieRepository repository) => _repository = repository;

    public async Task<IEnumerable<SerieResponseDto>> GetAllAsync()
    {
        var series = await _repository.GetWithGeneroAsync();
        return series.Select(ToDto);
    }

    public async Task<SerieResponseDto?> GetByIdAsync(int id)
    {
        var serie = await _repository.GetByIdAsync(id);
        return serie is null ? null : ToDto(serie);
    }

    public async Task<SerieResponseDto> CreateAsync(SerieRequestDto dto)
    {
        var entity = FromDto(dto);
        var created = await _repository.CreateAsync(entity);
        return ToDto(created);
    }

    public async Task<SerieResponseDto?> UpdateAsync(int id, SerieRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Titulo = dto.Titulo;
        existing.Sinopse = dto.Sinopse;
        existing.NumTemporadas = dto.NumTemporadas;
        existing.DataEstreia = dto.DataEstreia;
        existing.PosterUrl = dto.PosterUrl;
        existing.GeneroId = dto.GeneroId;

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

    public async Task<IEnumerable<SerieResponseDto>> GetByGeneroAsync(int generoId)
    {
        var series = await _repository.GetByGeneroAsync(generoId);
        return series.Select(ToDto);
    }

    public async Task<IEnumerable<SerieResponseDto>> SearchByTituloAsync(string titulo)
    {
        var series = await _repository.SearchByTituloAsync(titulo);
        return series.Select(ToDto);
    }

    private static Serie FromDto(SerieRequestDto dto) => new()
    {
        Titulo = dto.Titulo,
        Sinopse = dto.Sinopse,
        NumTemporadas = dto.NumTemporadas,
        DataEstreia = dto.DataEstreia,
        PosterUrl = dto.PosterUrl,
        GeneroId = dto.GeneroId
    };

    private static SerieResponseDto ToDto(Serie s) => new()
    {
        Id = s.Id,
        Titulo = s.Titulo,
        Sinopse = s.Sinopse,
        NumTemporadas = s.NumTemporadas,
        DataEstreia = s.DataEstreia,
        NotaMedia = s.NotaMedia,
        PosterUrl = s.PosterUrl,
        GeneroId = s.GeneroId,
        GeneroNome = s.Genero?.Nome
    };
}
