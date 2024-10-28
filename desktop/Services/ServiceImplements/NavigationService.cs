using desktop.Services.IServices;
using desktop.ViewModels.Base;

namespace desktop.Services.Implements;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    private BaseViewModel _currentViewModel;
    public BaseViewModel CurrentViewModel => _currentViewModel;

    private readonly Dictionary<Type, BaseViewModel> _viewModels;
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _viewModels = new Dictionary<Type, BaseViewModel>();
    }

    public event Action<BaseViewModel> OnViewModelChanged;

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        var viewModelType = typeof(TViewModel);
        if (!_viewModels.ContainsKey(viewModelType))
        {
            var viewModel = _serviceProvider.GetService(viewModelType) as BaseViewModel;
            _viewModels[viewModelType] = viewModel ?? throw new InvalidOperationException($"Could not resolve ViewModel of type {viewModelType.FullName}");
            _currentViewModel = viewModel;
        }
        OnViewModelChanged?.Invoke(_viewModels[viewModelType]);
    }
}