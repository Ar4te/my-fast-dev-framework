using System.Windows;
using desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}