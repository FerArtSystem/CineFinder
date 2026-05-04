using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface ISerieRepository : IBaseRepository<Serie>
{
    Task<IEnumerable<Serie>> GetByGeneroAsync(int generoId);
    Task<IEnumerable<Serie>> SearchByTituloAsync(string titulo);
    Task<IEnumerable<Serie>> GetWithGeneroAsync();
}
