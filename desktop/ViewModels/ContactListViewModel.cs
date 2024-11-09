using System.Collections.ObjectModel;
using System.IO;
using desktop.Models;
using desktop.Services.IServices;
using desktop.ViewModels.Base;
using HandyControl.Controls;

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
            OnPropertyChanged(nameof(CurrentPage));
            UpdateContacts();

            NextPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
        }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }

    public ObservableCollection<Contact> AllContacts { get; set; }

    public ObservableCollection<Contact> Contacts { get; set; }

    public RelayCommand<Contact> EditCommand { get; }
    public RelayCommand<Contact> DeleteCommand { get; }
    public RelayCommand<object> ExportToCSVCommand { get; }
    public RelayCommand NextPageCommand { get; }
    public RelayCommand PreviousPageCommand { get; }

    public ContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        AllContacts = new ObservableCollection<Contact>();
        Contacts = new ObservableCollection<Contact>();

        NextPageCommand = new RelayCommand(NextPage, () => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(PreviousPage, () => CurrentPage > 1);
        EditCommand = new RelayCommand<Contact>(EditContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact);
        ExportToCSVCommand = new RelayCommand<object>(ExportToCSV);
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

    private static void ExportToCSV(object list)
    {
        if (list is ObservableCollection<Contact> _contacts && _contacts.Count > 0)
        {
            var fileContent = string.Join("\r\n", _contacts.Select(t => $"{t.Id},{t.Name},{t.Email},{t.Phone}"));
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportFiles");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            bool isAppend = !File.Exists(filePath);
            using var writer = new StreamWriter(filePath, true);

            if (isAppend) writer.WriteLine("Id,Name,Email,Phone");
            writer.Write(fileContent);

            Growl.Success($"当前页面内容已保存到文件{filePath}");
        }
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