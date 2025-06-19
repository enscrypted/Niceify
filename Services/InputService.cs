using SharpHook;
using SharpHook.Native;
using System;
using System.Threading.Tasks;

namespace Niceify.Services
{
    public class InputService
    {
        private readonly IGlobalHook _hook;
        private readonly bool _isMac;

        public event Func<Task>? OnHotkeyPressed;

        public InputService(bool isMacPlatform)
        {
            _isMac = isMacPlatform;
            _hook = new SimpleGlobalHook();
            _hook.KeyPressed += async (sender, e) =>
            {
                if (IsHotkey(e))
                {
                    if (OnHotkeyPressed != null)
                        await OnHotkeyPressed.Invoke();
                }
            };
        }

        private bool IsHotkey(KeyboardHookEventArgs e)
        {
            var rawEvent = e.RawEvent; // Access the raw event data
            var expectedModifier = _isMac ? ModifierMask.Meta : ModifierMask.LeftCtrl | ModifierMask.RightCtrl;
            return (e.Data.KeyCode == KeyCode.VcRightShift || e.Data.KeyCode == KeyCode.VcLeftShift) && (rawEvent.Mask & expectedModifier) != 0;
        }

        public async Task RunAsync() => await _hook.RunAsync();
    }
}