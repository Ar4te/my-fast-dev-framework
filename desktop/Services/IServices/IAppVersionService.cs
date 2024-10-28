using desktop.Models;
using desktop.Services.Base;

namespace desktop.Services.IServices;

public interface IAppVersionService : IBaseService<AppVersion>
{
    Task<string> GetCurrentVersion();
}