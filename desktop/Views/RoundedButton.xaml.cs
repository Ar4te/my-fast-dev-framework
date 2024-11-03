using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace desktop.Views;

public partial class RoundedButton : UserControl
{
    public RoundedButton()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(RoundedButton),
        new PropertyMetadata(null));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public static readonly DependencyProperty BackgroundImageProperty =
        DependencyProperty.Register(nameof(BackgroundImage), typeof(ImageSource), typeof(RoundedButton), new PropertyMetadata(null));

    public ImageSource BackgroundImage
    {
        get => (ImageSource)GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }
}