using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using desktop.Views;

namespace desktop.ViewModels;

public class ViewModelViewSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is null) return null;
        var viewModelName = item.GetType().Name;
        var viewName = viewModelName.Replace("ViewModel", "View");

        var viewType = Type.GetType($"{typeof(BaseView).Namespace}.{viewName}");

        if (viewType != null)
        {
            var view = App.ServiceProvider.GetService(viewType);
            return view is null ? throw new InvalidOperationException($"{viewType} is not be registered") : new DataTemplate { VisualTree = new FrameworkElementFactory(view.GetType()) };
        }

        return base.SelectTemplate(item, container);
    }
}

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