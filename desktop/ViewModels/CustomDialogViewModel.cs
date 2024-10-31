using desktop.ViewModels.Base;

namespace desktop.ViewModels;

public class CustomDialogViewModel : BaseViewModel
{
    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private BaseViewModel _content;

    public BaseViewModel Content
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand CloseCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand ConfirmCommand { get; }

    public CustomDialogViewModel(string title, Action closeCommand, Action cancelCommand, Action confirmCommand)
    {
        Title = title;
        CloseCommand = new RelayCommand(closeCommand);
        CancelCommand = new RelayCommand(cancelCommand);
        ConfirmCommand = new RelayCommand(confirmCommand);
    }
}