using System.Globalization;
using System.Windows.Data;

namespace desktop.ViewModels.Converters;

public class CustomHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        if (value is double originHeight)
        {
            return originHeight - 2;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        throw new NotImplementedException("CustomHeightConverter 只支持单向转换。");
    }
}