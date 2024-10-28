using System.Windows.Controls;
using desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.Views;

public partial class StatusPotsView : UserControl
{
    public StatusPotsView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<StatusPotsViewModel>();
    }
}
