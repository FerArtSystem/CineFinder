using CineFinder.API.DTOs.Genero;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Interfaces;

namespace CineFinder.API.Services.Implementations;

public class GeneroService : IGeneroService
{
    private readonly IGeneroRepository _repository;

    public GeneroService(IGeneroRepository repository) => _repository = repository;

    public async Task<IEnumerable<GeneroResponseDto>> GetAllAsync()
    {
        var generos = await _repository.GetAllAsync();
        return generos.Select(ToDto);
    }

    public async Task<GeneroResponseDto?> GetByIdAsync(int id)
    {
        var genero = await _repository.GetByIdAsync(id);
        return genero is null ? null : ToDto(genero);
    }

    public async Task<GeneroResponseDto> CreateAsync(GeneroRequestDto dto)
    {
        var entity = new Genero { Nome = dto.Nome };
        var created = await _repository.CreateAsync(entity);
        return ToDto(created);
    }

    public async Task<GeneroResponseDto?> UpdateAsync(int id, GeneroRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;
        existing.Nome = dto.Nome;
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

    private static GeneroResponseDto ToDto(Genero g) => new() { Id = g.Id, Nome = g.Nome };
}
