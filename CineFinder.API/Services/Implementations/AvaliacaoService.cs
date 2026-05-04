using CineFinder.API.DTOs.Avaliacao;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _repository;

    public AvaliacaoService(IAvaliacaoRepository repository) => _repository = repository;

    public async Task<IEnumerable<AvaliacaoResponseDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToDto);
    }

    public async Task<AvaliacaoResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : ToDto(item);
    }

    public async Task<AvaliacaoResponseDto> CreateAsync(AvaliacaoRequestDto dto)
    {
        var entity = new Avaliacao
        {
            UsuarioId = dto.UsuarioId,
            FilmeId = dto.FilmeId,
            SerieId = dto.SerieId,
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            DataCriacao = DateTime.UtcNow
        };
        var created = await _repository.CreateAsync(entity);
        var withNav = await _repository.GetByIdAsync(created.Id);
        return ToDto(withNav!);
    }

    public async Task<AvaliacaoResponseDto?> UpdateAsync(int id, AvaliacaoRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Nota = dto.Nota;
        existing.Comentario = dto.Comentario;

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

    public async Task<IEnumerable<AvaliacaoResponseDto>> GetByFilmeAsync(int filmeId)
    {
        var items = await _repository.GetByFilmeAsync(filmeId);
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<AvaliacaoResponseDto>> GetBySerieAsync(int serieId)
    {
        var items = await _repository.GetBySerieAsync(serieId);
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<AvaliacaoResponseDto>> GetByUsuarioAsync(int usuarioId)
    {
        var items = await _repository.GetByUsuarioAsync(usuarioId);
        return items.Select(ToDto);
    }

    private static AvaliacaoResponseDto ToDto(Avaliacao a) => new()
    {
        Id = a.Id,
        UsuarioId = a.UsuarioId,
        UsuarioNickname = a.Usuario?.Nickname ?? string.Empty,
        FilmeId = a.FilmeId,
        FilmeTitulo = a.Filme?.Titulo,
        SerieId = a.SerieId,
        SerieTitulo = a.Serie?.Titulo,
        Nota = a.Nota,
        Comentario = a.Comentario,
        DataCriacao = a.DataCriacao
    };
}
