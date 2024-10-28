using Domain.Entity;

namespace Application.IService;

public interface IUserService : IBaseService<User>
{
    Task<bool> CreateUser(string userName, string password, string email);
    Task<string> Login(string userId, string password);
}
