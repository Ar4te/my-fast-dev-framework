using System.Collections.ObjectModel;
using desktop.Models;
using desktop.Services.IServices;
using desktop.ViewModels.Base;

namespace desktop.ViewModels;

public class ContactListViewModel : BaseViewModel
{
    private readonly IContactService _contactService;

    public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

    public ContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        _ = LoadContacts();
    }

    private async Task LoadContacts()
    {
        var contacts = await _contactService.GetListAsync();
        if (contacts != null && contacts.Any())
        {
            Contacts.Clear();
            foreach (var contact in contacts)
            {
                Contacts.Add(contact);
            }
        }
    }
}