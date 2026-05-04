using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IGeneroRepository : IBaseRepository<Genero>
{
    Task<Genero?> GetByNomeAsync(string nome);
}
