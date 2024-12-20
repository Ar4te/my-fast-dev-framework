﻿using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using desktop.Models;
using desktop.Services.IServices;
using desktop.ViewModels;
using HandyControl.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private static async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Growl.Info("MainWindow Loaded", "GlobalMsg");
        //TODO:启动时检查更新，待测试
        var curVersion = await App.ServiceProvider.GetRequiredService<IAppVersionService>().GetCurrentVersion();
        try
        {
            var serverVersion = await GetServerVersionAsync(); // 从服务器获取版本号
            if (serverVersion != curVersion &&
                MessageBox.Show("有新版本可用，是否更新？", "更新", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // 下载更新
                await DownloadUpdateAsync();
                // 安装更新
                InstallUpdate();
            }
        }
        catch
        {
            return;
        }

        await Task.CompletedTask;
    }

    private static async Task<string> GetServerVersionAsync()
    {
        var serverUrl = App.Configuration.GetValue("GetServerVersionUrl", "http://example.com/version.txt");
        using var client = new HttpClient();
        var serverVersionDto = await client.GetFromJsonAsync<ServerVersionDto>(serverUrl);
        return serverVersionDto is null ? "" : serverVersionDto.Version;
    }

    private static async Task DownloadUpdateAsync()
    {
        var serverUrl = App.Configuration.GetValue("DownloadUpdateZipUrl", "http://example.com/update.zip");
        using var client = new HttpClient();
        var response = await client.GetAsync(serverUrl);
        await using var fs = new FileStream("update.zip", FileMode.Create);
        await response.Content.CopyToAsync(fs);
    }

    private static void InstallUpdate()
    {
        Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater.exe"));
        Application.Current.Shutdown();
    }

    private void OpenDialog(object sender, RoutedEventArgs routedEventArgs)
    {
        var dialog = App.ServiceProvider.GetRequiredService<CustomDialog>();
        dialog.DataContext = new CustomDialogViewModel(
            "Test",
            closeCommand: () => dialog.Close(),
            cancelCommand: () => dialog.Close(),
            confirmCommand: () =>
            {
                MessageBox.Show("confirmed!");
                dialog.Close();
            });
        dialog.DialogContent = new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "This is a sample content inside the dialog." },
                new System.Windows.Controls.TextBox { Width = 200, Margin = new Thickness(5) }
            }
        };
        dialog.Owner = this;
        dialog.ShowDialog();
    }
}