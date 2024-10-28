using System.Windows.Controls;
using desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.Views;

public partial class AddContactView : UserControl
{
    public AddContactView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<AddContactViewModel>();
    }
}