using System;
using SharpHook.Native;

namespace SpencerAutoClicker.Source.Model
{
    public static class ClickerSettings
    {
        // Events
        public static event EventHandler<string> OnHotkeyChanged;

        // Internal config values
        private static Hotkey _hotkey;

        // Config values
        public static Hotkey Hotkey
        {
            get => _hotkey;
            set
            {
                _hotkey = value;
                OnHotkeyChanged?.Invoke(null, value.ToString());
            }
        } // determines the key used to start/stop the clicker
        public static int ClickInterval { get; set; } // determines delay between input up/down 
        public static bool ShouldHoldDown { get; set; } // determine if key should be clicked down but not up

        // Constructor
        static ClickerSettings()
        {
            Hotkey = new Hotkey(KeyCode.VcF9);
            ClickInterval = 50;
            ShouldHoldDown = false;
        }
    }
}
