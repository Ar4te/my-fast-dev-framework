using desktop.ViewModels.Base;

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

    public StatusPotsViewModel()
    {
        IsBlinking = true;
    }
}
