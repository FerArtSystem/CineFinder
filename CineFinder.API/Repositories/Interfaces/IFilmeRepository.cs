using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IFilmeRepository : IBaseRepository<Filme>
{
    Task<IEnumerable<Filme>> GetByGeneroAsync(int generoId);
    Task<IEnumerable<Filme>> SearchByTituloAsync(string titulo);
    Task<IEnumerable<Filme>> GetWithGeneroAsync();
}
