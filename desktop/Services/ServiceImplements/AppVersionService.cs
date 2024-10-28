using System.IO;
using System.Text.Json;
using desktop.DataAccess.Repository;
using desktop.Models;
using desktop.Services.Base;
using desktop.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace desktop.Services.Implements;

public class AppVersionService : BaseService<AppVersion>, IAppVersionService
{
    public AppVersionService(IBaseRepository<AppVersion> appVersionRepo)
    {
        BaseRepo = appVersionRepo;
    }

    public async Task<string> GetCurrentVersion()
    {
        var versionInfoFromFile = await GetVersionInfoFromFile();
        var appVersion = await BaseRepo.Client.Queryable<AppVersion>().OrderByDescending(t => t.Id).FirstAsync();

        if (appVersion is null && versionInfoFromFile is not null || (appVersion is not null && versionInfoFromFile is not null && versionInfoFromFile.ReleaseDate > appVersion.ReleaseDate))
        {
            var _appVersion = new AppVersion()
            {
                Version = versionInfoFromFile.Version,
                ReleaseDate = versionInfoFromFile.ReleaseDate,
                UpdateTime = DateTime.Now,
            };
            var tPk = await BaseRepo.CreateAndReturnPkAsync<int>(_appVersion);
            return tPk <= 0 ? "0.0.0" : versionInfoFromFile.Version;
        }

        if (appVersion is not null && (versionInfoFromFile is null || versionInfoFromFile.ReleaseDate <= appVersion.ReleaseDate))
            return appVersion.Version;

        return "0.0.0";

        //if (appVersion is null)
        //{
        //    if (versionInfoFromFile is null) return "0.0.0";
        //    var _appVersion = new AppVersion()
        //    {
        //        Version = versionInfoFromFile.Version,
        //        ReleaseDate = versionInfoFromFile.ReleaseDate,
        //        UpdateTime = DateTime.Now,
        //    };
        //    var tPk = await BaseRepo.CreateAndReturnPkAsync<int>(_appVersion);
        //    return tPk <= 0 ? "0.0.0" : versionInfoFromFile.Version;
        //}
        //else
        //{
        //    if (versionInfoFromFile is null || versionInfoFromFile.ReleaseDate <= appVersion.ReleaseDate) return appVersion.Version;
        //    var _appVersion = new AppVersion()
        //    {
        //        Version = versionInfoFromFile.Version,
        //        ReleaseDate = versionInfoFromFile.ReleaseDate,
        //        UpdateTime = DateTime.Now,
        //    };
        //    var tPk = await BaseRepo.CreateAndReturnPkAsync<int>(_appVersion);
        //    return tPk <= 0 ? "0.0.0" : versionInfoFromFile.Version;
        //}
    }

    private static async Task<ServerVersionDto?> GetVersionInfoFromFile()
    {
        var versionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.json");
        if (File.Exists(versionFilePath) && await File.ReadAllTextAsync(versionFilePath) is string versionJson && !string.IsNullOrEmpty(versionJson) && JsonConvert.DeserializeObject<ServerVersionDto>(versionJson) is ServerVersionDto versionInfoInFile && !string.IsNullOrEmpty(versionInfoInFile.Version))
        {
            return versionInfoInFile;
        }
        return null;
    }
}