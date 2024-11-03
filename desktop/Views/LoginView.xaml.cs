using System.Windows.Controls;
using desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
    }
}