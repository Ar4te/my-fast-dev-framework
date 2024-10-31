using System.Reflection;
using System.Windows;
using desktop.DataAccess.Repository;
using desktop.DataAccess.UnitOfWork;
using desktop.Exntesions;
using desktop.Models.Base;
using desktop.Services.Base;
using desktop.Services.Implements;
using desktop.Services.IServices;
using desktop.ViewModels;
using desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static IConfiguration Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\app.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30,
                retainedFileTimeLimit: TimeSpan.FromDays(10))
            .CreateLogger();
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
        ServiceProvider.InitialDb(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseEntity))).ToArray());
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSqlSugarSetup(Configuration);
        services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddSingleton<IContactService, ContactService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAppVersionService, AppVersionService>();

        services.AddSingleton<MainViewModel>();
        services.AddTransient<ContactListViewModel>();
        services.AddTransient<AddContactViewModel>();
        services.AddTransient<StatusPotsViewModel>();
        services.AddTransient<CustomDialog>();
        services.AddTransient<ContactListView>();
        services.AddTransient<AddContactView>();
        services.AddTransient<StatusPotsView>();

        services.AddSingleton<MainWindow>();
    }
}