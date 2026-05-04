using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IEnumerable<Post>> GetByUsuarioAsync(int usuarioId);
    Task<IEnumerable<Post>> GetRecentesAsync(int quantidade = 20);
}
