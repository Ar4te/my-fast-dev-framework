using desktop.Services.IServices;
using desktop.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.ViewModels;

public class MainViewModel : BaseViewModel
{
    public RelayCommand NavigateToContactListCommand { get; }
    public RelayCommand NavigateToAddContactCommand { get; }
    public RelayCommand NavigateToStatusPotsCommand { get; }

    public StatusPotsViewModel StatusPotsViewModel
    {
        get
        {
            return App.ServiceProvider.GetRequiredService<StatusPotsViewModel>();
        }
    }

    private BaseViewModel _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public MainViewModel(INavigationService navigationService)
    {
        NavigateToContactListCommand = new RelayCommand(navigationService.NavigateTo<ContactListViewModel>);
        NavigateToAddContactCommand = new RelayCommand(navigationService.NavigateTo<AddContactViewModel>);
        NavigateToStatusPotsCommand = new RelayCommand(navigationService.NavigateTo<StatusPotsViewModel>);

        navigationService.OnViewModelChanged += viewModel => CurrentViewModel = viewModel;
        navigationService.NavigateTo<ContactListViewModel>();
    }
}