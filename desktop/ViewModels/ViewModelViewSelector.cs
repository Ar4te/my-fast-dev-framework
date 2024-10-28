using System.Windows;
using System.Windows.Controls;
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
