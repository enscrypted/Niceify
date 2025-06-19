using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using System.Threading.Tasks;

namespace Niceify.Services
{
    public static class ClipboardService
    {
        private static IClipboard? GetClipboard()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow?.Clipboard;
            }

            return null;
        }

        public static async Task<string> GetTextAsync()
        {
            var clipboard = GetClipboard();
            return clipboard != null ? await clipboard.GetTextAsync() : string.Empty;
        }

        public static async Task SetTextAsync(string text)
        {
            var clipboard = GetClipboard();
            if (clipboard != null)
            {
                await clipboard.SetTextAsync(text);
            }
        }
    }
}