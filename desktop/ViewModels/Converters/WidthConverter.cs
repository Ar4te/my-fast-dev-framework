using System.Globalization;
using System.Windows.Data;

namespace desktop.ViewModels.Converters;

public class WidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 200 : 40; // 展开时宽度 200，收起时宽度 40
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}