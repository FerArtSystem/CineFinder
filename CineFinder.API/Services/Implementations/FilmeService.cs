using CineFinder.API.DTOs.Filme;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class FilmeService : IFilmeService
{
    private readonly IFilmeRepository _repository;

    public FilmeService(IFilmeRepository repository) => _repository = repository;

    public async Task<IEnumerable<FilmeResponseDto>> GetAllAsync()
    {
        var filmes = await _repository.GetWithGeneroAsync();
        return filmes.Select(ToDto);
    }

    public async Task<FilmeResponseDto?> GetByIdAsync(int id)
    {
        var filme = await _repository.GetByIdAsync(id);
        return filme is null ? null : ToDto(filme);
    }

    public async Task<FilmeResponseDto> CreateAsync(FilmeRequestDto dto)
    {
        var entity = FromDto(dto);
        var created = await _repository.CreateAsync(entity);
        return ToDto(created);
    }

    public async Task<FilmeResponseDto?> UpdateAsync(int id, FilmeRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Titulo = dto.Titulo;
        existing.Sinopse = dto.Sinopse;
        existing.DataLancamento = dto.DataLancamento;
        existing.DuracaoMinutos = dto.DuracaoMinutos;
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

    public async Task<IEnumerable<FilmeResponseDto>> GetByGeneroAsync(int generoId)
    {
        var filmes = await _repository.GetByGeneroAsync(generoId);
        return filmes.Select(ToDto);
    }

    public async Task<IEnumerable<FilmeResponseDto>> SearchByTituloAsync(string titulo)
    {
        var filmes = await _repository.SearchByTituloAsync(titulo);
        return filmes.Select(ToDto);
    }

    private static Filme FromDto(FilmeRequestDto dto) => new()
    {
        Titulo = dto.Titulo,
        Sinopse = dto.Sinopse,
        DataLancamento = dto.DataLancamento,
        DuracaoMinutos = dto.DuracaoMinutos,
        PosterUrl = dto.PosterUrl,
        GeneroId = dto.GeneroId
    };

    private static FilmeResponseDto ToDto(Filme f) => new()
    {
        Id = f.Id,
        Titulo = f.Titulo,
        Sinopse = f.Sinopse,
        DataLancamento = f.DataLancamento,
        DuracaoMinutos = f.DuracaoMinutos,
        NotaMedia = f.NotaMedia,
        PosterUrl = f.PosterUrl,
        GeneroId = f.GeneroId,
        GeneroNome = f.Genero?.Nome
    };
}
