using CineFinder.API.Models;

namespace CineFinder.API.Repositories.Interfaces;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> NicknameExistsAsync(string nickname);
}
