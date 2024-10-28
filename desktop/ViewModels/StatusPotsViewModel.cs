using desktop.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace desktop.ViewModels;

public class StatusPotsViewModel : BaseViewModel
{
    private bool _isBlinking;

    public bool IsBlinking
    {
        get => _isBlinking;
        set
        {
            _isBlinking = value;
            OnPropertyChanged();
        }
    }

    private bool _isRedBlinking;

    public bool IsRedBlinking
    {
        get => _isRedBlinking;
        set
        {
            _isRedBlinking = value;
            OnPropertyChanged();
        }
    }

    private bool _isGreenBlinking;

    public bool IsGreenBlinking
    {
        get => _isGreenBlinking;
        set
        {
            _isGreenBlinking = value;
            OnPropertyChanged();
        }
    }

    private bool _isGrayBlinking;

    public bool IsGrayBlinking
    {
        get => _isGrayBlinking;
        set
        {
            _isGrayBlinking = value;
            OnPropertyChanged();
        }
    }

    public StatusPotsViewModel()
    {
        IsGrayBlinking = true;
        IsGreenBlinking = true;
        IsRedBlinking = true;
    }

    // public static void Success()
    // {
    //     var statusPotsViewModel = App.ServiceProvider.GetRequiredService<StatusPotsViewModel>();
    //     statusPotsViewModel.IsGreenBlinking = true;
    // }
}