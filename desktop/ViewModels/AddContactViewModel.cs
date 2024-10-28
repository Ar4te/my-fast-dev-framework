using System.Windows.Input;
using desktop.Models;
using desktop.Services.IServices;
using desktop.ViewModels.Base;
using HandyControl.Controls;

namespace desktop.ViewModels;

public class AddContactViewModel : BaseViewModel
{
    private readonly IContactService _contactService;
    private readonly INavigationService _navigationService;

    public Contact NewContact { get; set; }
    public RelayCommand SaveContactCommand { get; }

    public AddContactViewModel(IContactService contactService, INavigationService navigationService)
    {
        NewContact = new Contact();
        _contactService = contactService;
        _navigationService = navigationService;
        SaveContactCommand = new RelayCommand(async () => await SaveContact());
    }

    private async Task SaveContact()
    {
        var row = await _contactService.CreateAsync(NewContact);
        if (row == 1)
        {
            Growl.Success("操作成功", "GlobalMsg");
            // StatusPotsViewModel.Success();
            _navigationService.NavigateTo<ContactListViewModel>();
        }
        else
        {
            Growl.Error("操作失败", "GlobalMsg");
        }
    }
}