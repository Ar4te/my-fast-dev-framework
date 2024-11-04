using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace desktop.Behaviors;

public class HighlighterBehavior
{
    public static readonly DependencyProperty IsHighlightedPropertyProperty =
        DependencyProperty.Register("IsHighlightedProperty", typeof(bool), typeof(HighlighterBehavior), new PropertyMetadata(false, OnIsHighlightedChanged));

    private static void OnIsHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public static bool GetIsHighlighted(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsHighlightedPropertyProperty);
    }

    public static void SetIsHighlighted(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is UIElement uiElement)
        {
            if ((bool)e.NewValue)
            {
                uiElement.SetValue(Control.BackgroundProperty, Brushes.Yellow);
            }
            else
            {
                uiElement.SetValue(Control.BackgroundProperty, Brushes.Transparent);
            }
        }
    }
}
