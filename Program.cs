using Avalonia;
using System;
using System.Reactive.Concurrency;
using System.Threading;
using Avalonia.Threading;
using ReactiveUI;

namespace Niceify
{
    class Program
    {
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}