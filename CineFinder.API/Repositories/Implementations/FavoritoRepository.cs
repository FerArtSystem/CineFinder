using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class FavoritoRepository : IFavoritoRepository
{
    private readonly AppDbContext _context;

    public FavoritoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Favorito>> GetAllAsync() =>
        await _context.Favoritos
            .Include(f => f.Usuario)
            .Include(f => f.Filme)
            .Include(f => f.Serie)
            .AsNoTracking().ToListAsync();

    public async Task<Favorito?> GetByIdAsync(int id) =>
        await _context.Favoritos
            .Include(f => f.Usuario)
            .Include(f => f.Filme)
            .Include(f => f.Serie)
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<IEnumerable<Favorito>> GetByUsuarioAsync(int usuarioId) =>
        await _context.Favoritos
            .Where(f => f.UsuarioId == usuarioId)
            .Include(f => f.Filme)
            .Include(f => f.Serie)
            .AsNoTracking().ToListAsync();

    public async Task<Favorito?> GetByUsuarioAndFilmeAsync(int usuarioId, int filmeId) =>
        await _context.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);

    public async Task<Favorito?> GetByUsuarioAndSerieAsync(int usuarioId, int serieId) =>
        await _context.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.SerieId == serieId);

    public async Task<Favorito> CreateAsync(Favorito entity)
    {
        _context.Favoritos.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Favorito> UpdateAsync(Favorito entity)
    {
        _context.Favoritos.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Favoritos.FindAsync(id);
        if (entity is not null)
        {
            _context.Favoritos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
