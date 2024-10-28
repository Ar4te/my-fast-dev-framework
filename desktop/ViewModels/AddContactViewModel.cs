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
    public ICommand SaveContactCommand { get; }

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
            Growl.Success("456456", "SuccessMsg");
            _navigationService.NavigateTo<ContactListViewModel>();
        }
        else
        {
            Growl.Error("123123", "SuccessMsg");
        }
    }
}