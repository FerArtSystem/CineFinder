using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class GeneroRepository : IGeneroRepository
{
    private readonly AppDbContext _context;

    public GeneroRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Genero>> GetAllAsync() =>
        await _context.Generos.AsNoTracking().ToListAsync();

    public async Task<Genero?> GetByIdAsync(int id) =>
        await _context.Generos.FindAsync(id);

    public async Task<Genero?> GetByNomeAsync(string nome) =>
        await _context.Generos.FirstOrDefaultAsync(g => g.Nome == nome);

    public async Task<Genero> CreateAsync(Genero entity)
    {
        _context.Generos.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Genero> UpdateAsync(Genero entity)
    {
        _context.Generos.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Generos.FindAsync(id);
        if (entity is not null)
        {
            _context.Generos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
