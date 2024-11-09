using System.Globalization;
using System.Windows.Data;

namespace desktop.ViewModels.Converters;

public class PageEnabledConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length != 2)
            return false;

        if (values[0] is int currentPage && values[1] is int totalPages)
        {
            if (parameter != null && parameter.ToString() == "Next")
            {
                return currentPage < totalPages;
            }
            else if (parameter != null && parameter.ToString() == "Previous")
            {
                return currentPage > 1;
            }
        }

        return true;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
