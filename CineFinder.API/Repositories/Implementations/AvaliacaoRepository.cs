using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly AppDbContext _context;

    public AvaliacaoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Avaliacao>> GetAllAsync() =>
        await _context.Avaliacoes
            .Include(a => a.Usuario)
            .Include(a => a.Filme)
            .Include(a => a.Serie)
            .AsNoTracking().ToListAsync();

    public async Task<Avaliacao?> GetByIdAsync(int id) =>
        await _context.Avaliacoes
            .Include(a => a.Usuario)
            .Include(a => a.Filme)
            .Include(a => a.Serie)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Avaliacao>> GetByFilmeAsync(int filmeId) =>
        await _context.Avaliacoes
            .Where(a => a.FilmeId == filmeId)
            .Include(a => a.Usuario)
            .AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Avaliacao>> GetBySerieAsync(int serieId) =>
        await _context.Avaliacoes
            .Where(a => a.SerieId == serieId)
            .Include(a => a.Usuario)
            .AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Avaliacao>> GetByUsuarioAsync(int usuarioId) =>
        await _context.Avaliacoes
            .Where(a => a.UsuarioId == usuarioId)
            .Include(a => a.Filme)
            .Include(a => a.Serie)
            .AsNoTracking().ToListAsync();

    public async Task<double> GetMediaNotaFilmeAsync(int filmeId) =>
        await _context.Avaliacoes
            .Where(a => a.FilmeId == filmeId)
            .AverageAsync(a => (double?)a.Nota) ?? 0;

    public async Task<double> GetMediaNotaSerieAsync(int serieId) =>
        await _context.Avaliacoes
            .Where(a => a.SerieId == serieId)
            .AverageAsync(a => (double?)a.Nota) ?? 0;

    public async Task<Avaliacao> CreateAsync(Avaliacao entity)
    {
        _context.Avaliacoes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Avaliacao> UpdateAsync(Avaliacao entity)
    {
        _context.Avaliacoes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Avaliacoes.FindAsync(id);
        if (entity is not null)
        {
            _context.Avaliacoes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
