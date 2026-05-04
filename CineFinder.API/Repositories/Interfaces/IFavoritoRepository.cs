using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IFavoritoRepository : IBaseRepository<Favorito>
{
    Task<IEnumerable<Favorito>> GetByUsuarioAsync(int usuarioId);
    Task<Favorito?> GetByUsuarioAndFilmeAsync(int usuarioId, int filmeId);
    Task<Favorito?> GetByUsuarioAndSerieAsync(int usuarioId, int serieId);
}
