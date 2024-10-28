using System.Reflection;
using Common.CustomAttribute;
using Microsoft.Extensions.Options;
using SqlSugar;

namespace Presentation.Extension;

public static class SqlSugarSetup
{
    public static void AddSqlSugarSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfigs = configuration.GetSection("DbConfigs").Get<List<DbConfig>>();
        if (dbConfigs is null || dbConfigs.Count == 0) throw new ArgumentException(nameof(dbConfigs));
        var configs = new List<ConnectionConfig>();
        dbConfigs.ForEach(dbConfig =>
        {
            configs.Add(new ConnectionConfig
            {
                ConfigId = dbConfig.ConfigId,
                ConnectionString = dbConfig.ConnectionString,
                DbType = (DbType)dbConfig.DbType,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true,
                    SqlServerCodeFirstNvarchar = true,
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (property, column) =>
                    {
                        if (column.IsPrimarykey && property.PropertyType == typeof(int) && !column.IsIdentity)
                        {
                            column.IsIdentity = true;
                        }
                    }
                },
                InitKeyType = InitKeyType.Attribute
            });
        });

        services.Configure<List<ConnectionConfig>>(opt =>
        {
            opt.AddRange(configs);
        });
        services.AddSingleton<ISqlSugarClient>(opt =>
        {
            var sqlSugarScope = new SqlSugarScope(configs, sqlSugarClient =>
            {
                configs.ForEach(config =>
                {
                    var dbProvider = sqlSugarClient.GetConnectionScope(config.ConfigId);
                    dbProvider.Aop.OnLogExecuting = (sql, parameters) =>
                    {
                        Console.WriteLine(string.Format("------------------ \r\n ConnId:[{0}]【SQL语句】: \r\n {1}", config.ConfigId, UtilMethods.GetNativeSql(sql, parameters)));
                    };
                });
            });

            return sqlSugarScope;
        });
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="types">要同步结构的实体</param>
    public static void InitialDb(this IServiceProvider serviceProvider, params Type[] types)
    {
        using var scope = serviceProvider.CreateScope();
        var sqlSugarClient = scope.ServiceProvider.GetService<ISqlSugarClient>();
        using var sqlSugarScope = sqlSugarClient as SqlSugarScope;
        var connectionConfigs = scope.ServiceProvider.GetService<IOptions<List<ConnectionConfig>>>()!.Value;
        connectionConfigs.ForEach(config =>
        {
            if (!sqlSugarClient!.CurrentConnectionConfig.ConfigId.ToString()!.Equals(config.ConfigId.ToString(), StringComparison.Ordinal))
            {
                sqlSugarClient = sqlSugarScope!.GetConnectionScope(config.ConfigId);
            }

            try
            {
                if (!sqlSugarClient.DbMaintenance.GetDataBaseList().Exists(d => d.Equals(config.ConfigId.ToString(), StringComparison.Ordinal)))
                {
                    sqlSugarClient.DbMaintenance.CreateDatabase();
                }
            }
            catch (SqlSugarException)
            {
                sqlSugarClient.DbMaintenance.CreateDatabase();
            }

            var _types = types.Where(t => t.GetCustomAttribute<DatabaseConfigIdAttribute>() is { } dc && !string.IsNullOrEmpty(dc.ConfigId) && dc.ConfigId.Equals(config.ConfigId.ToString(), StringComparison.Ordinal)).ToArray();

            sqlSugarClient.CodeFirst.InitTables(_types);
        });
    }
}