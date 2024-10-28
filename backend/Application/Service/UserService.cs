using Application.IService;
using Common.Extension;
using Domain.Entity;
using Domain.IRepository;

namespace Application.Service;

public class UserService : BaseService<User>, IUserService
{
    private readonly IBaseRepository<Log> _logRepo;

    public UserService(IBaseRepository<User> baseRepo, IBaseRepository<Log> logRepo)
    {
        BaseRepo = baseRepo;
        _logRepo = logRepo;
    }

    public async Task<bool> CreateUser(string userName, string password, string email)
    {
        var user = new User
        {
            Name = userName,
            Email = email,
            PasswordHash = new Password(password).GetPasswordHash()
        };

        var res = await BaseRepo.CreateAsync(user) == 1;

        if (res)
        {
            var dt = DateTime.Now;
            var res1 = await _logRepo.CreateAsync(new Log
            {
                DateTime = dt,
                DatetimeStr = dt.ToString(),
                Message = $"Successfully created user {userName}"
            });
            return res1 == 1;
        }
        return res;
    }

    public async Task<string> Login(string userId, string password)
    {
        var user = await BaseRepo.GetByIdAsync(userId);
        if (user == null) return $"User {userId} is not exist";
        if (new Password(password).Verify(user.PasswordHash))
        {
            return "token";
        }
        return "Failed to check password";
    }
}