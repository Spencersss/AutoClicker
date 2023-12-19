using SharpHook;
using SharpHook.Native;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SpencerAutoClicker.Source.Backend
{
    public class ClickerHotkeyHook
    {
        private SimpleGlobalHook hook;
        private Action<object, RoutedEventArgs> _action;
        private SynchronizationContext synchronizationContext;

        public ClickerHotkeyHook(Action<object, RoutedEventArgs> action)
        {
            _action = action;
            synchronizationContext = SynchronizationContext.Current;

            hook = new SimpleGlobalHook();
            hook.KeyPressed += (sender, e) =>
            {
                synchronizationContext.Post(new SendOrPostCallback((obj) =>
                {
                    OnKeyPress(sender, e);
                }), null);
            };

            hook.RunAsync();
        }

        private void OnKeyPress(object sender, KeyboardHookEventArgs e)
        {
            if (e.Data.KeyCode == KeyCode.VcF9)
            {
                _action(null, null);
            }
        }
    }

}
