using CineFinder.API.DTOs.Favorito;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class FavoritoService : IFavoritoService
{
    private readonly IFavoritoRepository _repository;

    public FavoritoService(IFavoritoRepository repository) => _repository = repository;

    public async Task<IEnumerable<FavoritoResponseDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToDto);
    }

    public async Task<FavoritoResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : ToDto(item);
    }

    public async Task<FavoritoResponseDto> CreateAsync(FavoritoRequestDto dto)
    {
        var entity = new Favorito
        {
            UsuarioId = dto.UsuarioId,
            FilmeId = dto.FilmeId,
            SerieId = dto.SerieId,
            DataAdicionado = DateTime.UtcNow
        };
        var created = await _repository.CreateAsync(entity);
        var withNav = await _repository.GetByIdAsync(created.Id);
        return ToDto(withNav!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;
        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<FavoritoResponseDto>> GetByUsuarioAsync(int usuarioId)
    {
        var items = await _repository.GetByUsuarioAsync(usuarioId);
        return items.Select(ToDto);
    }

    private static FavoritoResponseDto ToDto(Favorito f) => new()
    {
        Id = f.Id,
        UsuarioId = f.UsuarioId,
        UsuarioNickname = f.Usuario?.Nickname ?? string.Empty,
        FilmeId = f.FilmeId,
        FilmeTitulo = f.Filme?.Titulo,
        SerieId = f.SerieId,
        SerieTitulo = f.Serie?.Titulo,
        DataAdicionado = f.DataAdicionado
    };
}
