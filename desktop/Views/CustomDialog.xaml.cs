﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace desktop.Views;

public partial class CustomDialog : Window
{
    public CustomDialog()
    {
        InitializeComponent();
        
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        ShowInTaskbar = false;
        Topmost = true;
        Owner = null;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        
        Loaded += (s, e) => FadeIn();
        MouseDown += CustomDialog_MouseDown;
    }

    private void CustomDialog_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void FadeIn()
    {
        var fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
        BeginAnimation(OpacityProperty, fadeInAnimation);
    }

    private bool isFirstClose = true;

    protected override async void OnClosing(CancelEventArgs e)
    {
        e.Cancel = isFirstClose;
        isFirstClose = false;
        var fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds((0.3)));
        fadeOutAnimation.Completed += (s, _) => Close();

        BeginAnimation(OpacityProperty, fadeOutAnimation);
    }

    private void CloseBtnMouseEnter(object sender, MouseEventArgs e)
    {
        CloseBtn.BorderBrush = Brushes.Gray;
    }

    private void CloseBtnMouseLeave(object sender, MouseEventArgs e)
    {
        CloseBtn.BorderBrush = Brushes.Transparent;
    }
}