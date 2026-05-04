using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IAvaliacaoRepository : IBaseRepository<Avaliacao>
{
    Task<IEnumerable<Avaliacao>> GetByFilmeAsync(int filmeId);
    Task<IEnumerable<Avaliacao>> GetBySerieAsync(int serieId);
    Task<IEnumerable<Avaliacao>> GetByUsuarioAsync(int usuarioId);
    Task<double> GetMediaNotaFilmeAsync(int filmeId);
    Task<double> GetMediaNotaSerieAsync(int serieId);
}
