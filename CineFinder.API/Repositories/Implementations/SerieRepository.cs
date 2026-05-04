using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class SerieRepository : ISerieRepository
{
    private readonly AppDbContext _context;

    public SerieRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Serie>> GetAllAsync() =>
        await _context.Series.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Serie>> GetWithGeneroAsync() =>
        await _context.Series.Include(s => s.Genero).AsNoTracking().ToListAsync();

    public async Task<Serie?> GetByIdAsync(int id) =>
        await _context.Series.Include(s => s.Genero).FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Serie>> GetByGeneroAsync(int generoId) =>
        await _context.Series.Where(s => s.GeneroId == generoId)
            .Include(s => s.Genero).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Serie>> SearchByTituloAsync(string titulo) =>
        await _context.Series
            .Where(s => s.Titulo.Contains(titulo))
            .Include(s => s.Genero)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Serie> CreateAsync(Serie entity)
    {
        _context.Series.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Serie> UpdateAsync(Serie entity)
    {
        _context.Series.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Series.FindAsync(id);
        if (entity is not null)
        {
            _context.Series.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
