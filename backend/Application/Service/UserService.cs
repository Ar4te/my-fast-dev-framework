using System.Security.Claims;
using Application.IService;
using Common.Extension;
using Common.Helper;
using Domain.Entity;
using Domain.Entity.RBAC;
using Domain.IRepository;

namespace Application.Service;

public class UserService : BaseService<User>, IUserService
{
    private readonly IBaseRepository<Log> _logRepo;
    private readonly JwtHelper _jwtHelper;

    public UserService(IBaseRepository<User> baseRepo, IBaseRepository<Log> logRepo, JwtHelper jwtHelper)
    {
        BaseRepo = baseRepo;
        _logRepo = logRepo;
        _jwtHelper = jwtHelper;
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

    public async Task<string> Login(string userName, string password)
    {
        var user = await BaseRepo.GetAsync(user => user.Name == userName);
        if (user == null) return $"User {userName} is not exist";
        if (new Password(password).Verify(user.PasswordHash))
        {
            var token = _jwtHelper.GetToken(new List<Claim>
            {
                new("UserId", user.Id.ToString()),
                new("UserName",user.Name)
            });
            return token;
        }
        return "Failed to check password";
    }
}