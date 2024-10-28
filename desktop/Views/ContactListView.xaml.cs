using System.Windows.Controls;
using desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.Views;

public partial class ContactListView : UserControl
{
    public ContactListView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<ContactListViewModel>();
    }
}
