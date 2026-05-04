using CineFinder.API.Data;
using CineFinder.API.Models;
using CineFinder.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.API.Repositories.Implementations;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Post>> GetAllAsync() =>
        await _context.Posts
            .Include(p => p.Usuario)
            .AsNoTracking().ToListAsync();

    public async Task<Post?> GetByIdAsync(int id) =>
        await _context.Posts
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Post>> GetByUsuarioAsync(int usuarioId) =>
        await _context.Posts
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataCriacao)
            .AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Post>> GetRecentesAsync(int quantidade = 20) =>
        await _context.Posts
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.DataCriacao)
            .Take(quantidade)
            .AsNoTracking().ToListAsync();

    public async Task<Post> CreateAsync(Post entity)
    {
        _context.Posts.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Post> UpdateAsync(Post entity)
    {
        _context.Posts.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Posts.FindAsync(id);
        if (entity is not null)
        {
            _context.Posts.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
