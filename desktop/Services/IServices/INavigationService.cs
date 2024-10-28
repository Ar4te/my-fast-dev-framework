using desktop.ViewModels.Base;

namespace desktop.Services.IServices;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    event Action<BaseViewModel> OnViewModelChanged;
}