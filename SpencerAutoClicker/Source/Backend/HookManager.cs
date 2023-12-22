using SharpHook;
using SharpHook.Native;
using SpencerAutoClicker.Source.Frontend;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SpencerAutoClicker.Source.Backend
{
    public class HookManager
    {
        // Events
        public event EventHandler<RoutedEventArgs> OnHotkeyTriggered;

        // Vars
        public SimpleGlobalHook Hook;

        // Constructor
        public HookManager()
        {
            Hook = new SimpleGlobalHook();
            Hook.KeyPressed += OnKeyPress;
            Hook.MousePressed += OnMousePress;

            Hook.RunAsync();
        }

        // Methods
        public void CleanupActiveHooks()
        {
            try
            {
                Hook.Dispose();
                Trace.WriteLine("Successfully destroyed");
            } catch (HookException e)
            {
                Trace.WriteLine(e);
            }
        }

        // Event handlers
        private void OnKeyPress(object sender, KeyboardHookEventArgs e)
        {
            if (e.Data.KeyCode == ClickerSettings.Hotkey.GetKeyCode())
            {
                OnHotkeyTriggered?.Invoke(null, null);
            }
        }

        private void OnMousePress(object sender, MouseHookEventArgs e)
        {
            if (e.Data.Button == ClickerSettings.Hotkey.GetMouseButton())
            {
                OnHotkeyTriggered?.Invoke(null, null);
            }
        }
    }

}
