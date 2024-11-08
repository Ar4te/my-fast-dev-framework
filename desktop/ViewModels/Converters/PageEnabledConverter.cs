using System.Globalization;
using System.Windows.Data;

namespace desktop.ViewModels.Converters;

public class PageEnabledConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length != 2)
            return false;

        var currentPage = (int)values[0];
        var totalPages = (int)values[1];

        if (parameter != null && parameter.ToString() == "Next")
        {
            var t = currentPage < totalPages;
            return t;
        }
        else if (parameter != null && parameter.ToString() == "Previous")
        {
            var t = currentPage > 1;
            return t;
        }

        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
