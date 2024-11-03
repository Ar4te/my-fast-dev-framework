using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace desktop.ViewModels.Converters;

public class CustomWindowStateToToggleCommandConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is WindowState windowState)
        {
            // 如果窗口是最大化的，则返回还原命令，否则返回最大化命令
            return windowState == WindowState.Maximized ?
                   SystemCommands.RestoreWindowCommand : SystemCommands.MaximizeWindowCommand;
        }
        return SystemCommands.MaximizeWindowCommand;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}