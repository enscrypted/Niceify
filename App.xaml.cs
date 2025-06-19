using System.IO;
using System.Reactive.Concurrency;
using System.Text.Json;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Niceify.Views;
using ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;

namespace Niceify
{
    public partial class App : Application
    {
        private TrayIcon? _trayIcon;
        private MainWindow? _mainWindow;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            RxApp.MainThreadScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _mainWindow = new MainWindow();
                desktop.MainWindow = _mainWindow;

                // Tray Icon Logic
                _trayIcon = new TrayIcon
                {
                    Icon = new WindowIcon("Assets/icon.ico"),
                    ToolTipText = "Niceify - Click to open settings",
                    IsVisible = true
                };

                // Tray Right-Click Menu
                var trayMenu = new NativeMenu();
                var openMenuItem = new NativeMenuItem { Header = "Open" };
                var exitMenuItem = new NativeMenuItem { Header = "Exit" };

                // Subscribe to click events for the tray menu items
                openMenuItem.Click += (_, _) => OpenWindow();
                exitMenuItem.Click += (_, _) => ExitApp();

                // Add items to the menu
                trayMenu.Items.Add(openMenuItem);
                trayMenu.Items.Add(exitMenuItem);

                // Set the tray icon menu
                _trayIcon.Menu = trayMenu;

                // Tray Icon Click Behavior
                _trayIcon.Clicked += (s, e) =>
                {
                    if (_mainWindow!.WindowState == WindowState.Minimized)
                    {
                        _mainWindow.WindowState = WindowState.Normal;
                    }

                    _mainWindow.Show();
                    _mainWindow.Activate();
                };

                // Hide window on close, keeping the tray icon
                _mainWindow.Closed += (_, _) => _mainWindow.Hide();
            }

            // Apply theme based on configuration
            ApplyThemeFromSettings();

            base.OnFrameworkInitializationCompleted();
        }

        private void OpenWindow()
        {
            if (_mainWindow != null && _mainWindow.WindowState == WindowState.Minimized)
            {
                _mainWindow.WindowState = WindowState.Normal;
            }

            _mainWindow?.Show();
            _mainWindow?.Activate();
        }

        private void ExitApp()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        private void ApplyThemeFromSettings()
        {
            var settingsPath = "appSettings.json";
            if (!File.Exists(settingsPath))
                return;

            var json = File.ReadAllText(settingsPath);
            var config = JsonSerializer.Deserialize<AppSettings>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            RequestedThemeVariant = config?.Theme == "Dark" ? ThemeVariant.Dark : ThemeVariant.Light;
        }

        private class AppSettings
        {
            public string Theme { get; set; } = "Light";
        }
    }
}
