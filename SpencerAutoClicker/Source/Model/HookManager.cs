using SharpHook;
using System;
using System.Diagnostics;
using System.Windows;

namespace SpencerAutoClicker.Source.Model
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
            }
            catch (HookException e)
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
