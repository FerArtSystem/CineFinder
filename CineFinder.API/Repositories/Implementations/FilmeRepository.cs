using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class FilmeRepository : IFilmeRepository
{
    private readonly AppDbContext _context;

    public FilmeRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Filme>> GetAllAsync() =>
        await _context.Filmes.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Filme>> GetWithGeneroAsync() =>
        await _context.Filmes.Include(f => f.Genero).AsNoTracking().ToListAsync();

    public async Task<Filme?> GetByIdAsync(int id) =>
        await _context.Filmes.Include(f => f.Genero).FirstOrDefaultAsync(f => f.Id == id);

    public async Task<IEnumerable<Filme>> GetByGeneroAsync(int generoId) =>
        await _context.Filmes.Where(f => f.GeneroId == generoId)
            .Include(f => f.Genero).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Filme>> SearchByTituloAsync(string titulo) =>
        await _context.Filmes
            .Where(f => f.Titulo.Contains(titulo))
            .Include(f => f.Genero)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Filme> CreateAsync(Filme entity)
    {
        _context.Filmes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Filme> UpdateAsync(Filme entity)
    {
        _context.Filmes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Filmes.FindAsync(id);
        if (entity is not null)
        {
            _context.Filmes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
