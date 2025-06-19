using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Niceify.Services;
using Niceify.ViewModels;
using SharpHook;
using SharpHook.Native;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Niceify.Views
{
    public partial class MainWindow : Window
    {
        private readonly InputService _inputService;
        private readonly AiService _aiService;
        private readonly bool isMac;

        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            DataContext = vm;

            isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            _inputService = new InputService(isMac);
            _aiService = new AiService();
            _inputService.OnHotkeyPressed += OnHotkeyTrigger;
            _inputService.RunAsync(); // fire and forget
            
            vm.SaveCommand.Subscribe(_ =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() => this.Hide());
            });
        }

        public async Task HideWindow()
        {
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                this.Hide();
            });
        }

        private async Task OnHotkeyTrigger()
        {
            try
            {
                var modifier = GetModifierKey();
                SimulateKeyStroke(modifier, KeyCode.VcA); // Ctrl/Cmd + A
                await Task.Delay(150);
                SimulateKeyStroke(modifier, KeyCode.VcX); // Ctrl/Cmd + X
                await Task.Delay(200);

                var originalText = await ClipboardService.GetTextAsync();
                if (string.IsNullOrWhiteSpace(originalText)) return;

                // 🧠 Grab view model safely on UI thread
                var vm = await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                    DataContext as MainWindowViewModel
                );

                var rewritten = await _aiService.RewriteTextAsync(originalText, vm.SelectedTone, vm.CustomPrompt);
                
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await ClipboardService.SetTextAsync(rewritten);
                    await Task.Delay(150);

                    SimulateKeyStroke(modifier, KeyCode.VcV); // Ctrl/Cmd + V
                    await Task.Delay(100);

                    if (vm.SendEnter)
                        SimulateKeyPress(KeyCode.VcEnter);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private KeyCode GetModifierKey() =>
            isMac ? KeyCode.VcLeftMeta : KeyCode.VcLeftControl;

        private void SimulateKeyStroke(KeyCode modifier, KeyCode key)
        {
            var sim = new EventSimulator();
            sim.SimulateKeyPress(modifier);
            sim.SimulateKeyPress(key);
            sim.SimulateKeyRelease(key);
            sim.SimulateKeyRelease(modifier);
        }

        private void SimulateKeyPress(KeyCode key)
        {
            var sim = new EventSimulator();
            sim.SimulateKeyPress(key);
            sim.SimulateKeyRelease(key);
        }

        private void OnCloseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Hide window instead of closing
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                this.Hide();
            });
        }
    }
}
