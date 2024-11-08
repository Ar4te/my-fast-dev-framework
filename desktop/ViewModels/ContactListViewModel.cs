using System.Collections.ObjectModel;
using desktop.Models;
using desktop.Services.IServices;
using desktop.ViewModels.Base;

namespace desktop.ViewModels;

public class ContactListViewModel : BaseViewModel
{
    private readonly IContactService _contactService;
    private int _currentPage = 1;
    private readonly int _pageSize = 10;

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
            UpdateContacts();
        }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Contact> AllContacts { get; set; }

    public ObservableCollection<Contact> Contacts { get; set; }

    public RelayCommand<Contact> EditCommand { get; }
    public RelayCommand<Contact> DeleteCommand { get; }
    public RelayCommand<List<Contact>> ExportToCSVCommand { get; }
    public RelayCommand<int> NextPageCommand { get; }
    public RelayCommand<int> PreviousPageCommand { get; }

    public ContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        AllContacts = new ObservableCollection<Contact>();
        Contacts = new ObservableCollection<Contact>();

        NextPageCommand = new RelayCommand<int>(_ => NextPage(), _ => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand<int>(_ => PreviousPage(), _ => CurrentPage > 1);
        EditCommand = new RelayCommand<Contact>(EditContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact);
        ExportToCSVCommand = new RelayCommand<List<Contact>>(ExportToCSV);
        _ = LoadContacts();
        UpdateContacts();
    }

    private async Task LoadContacts()
    {
        var contacts = await _contactService.GetListAsync();
        AllContacts.Clear();
        if (contacts != null && contacts.Any())
        {
            foreach (var contact in contacts)
            {
                AllContacts.Add(contact);
            }
        }
        TotalPages = AllContacts.Count <= 0 ? 1 : (int)Math.Ceiling((double)AllContacts.Count / _pageSize);
    }

    private void UpdateContacts()
    {
        Contacts.Clear();
        var items = AllContacts.Skip((_currentPage - 1) * _pageSize).Take(_pageSize);
        foreach (var item in items)
        {
            Contacts.Add(item);
        }
    }

    private void NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
        }
    }

    private void PreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
        }
    }

    private void ExportToCSV(List<Contact> list)
    {
        throw new NotImplementedException();
    }

    private void EditContact(Contact contact)
    {
        var t = contact.GetType();
    }

    private async void DeleteContact(Contact contact)
    {
        await _contactService.DeleteAsync(contact.Id);
        await LoadContacts();
    }
}