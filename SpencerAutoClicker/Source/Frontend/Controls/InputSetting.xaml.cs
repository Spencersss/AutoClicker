using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ninject;
using SharpHook;
using SharpHook.Native;
using SpencerAutoClicker.Source.Backend;
using SpencerAutoClicker.Source.Backend.Exceptions;
using SpencerAutoClicker.Source.Backend.Helpers;

namespace SpencerAutoClicker.Source.Frontend.Controls
{


    /// <summary>
    /// Interaction logic for HotkeySetting.xaml
    /// </summary>
    public partial class InputSetting : UserControl, INotifyPropertyChanged
    {
        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Vars
        private const string _defaultControlText = "Set";
        private string _controlText = "";
        private HookManager _hookManager;

        // State vars
        public bool CanClick = true;

        // Properties
        [Inject]
        public HookManager HookManager
        {
            get => _hookManager;
            set
            {
                _hookManager = value;
            }
        }

        public string ControlText
        {
            get => _controlText;
            set
            {
                _controlText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ControlText)));
            }
        }

        // State vars
        private bool _isWaitingForInput = false;

        // Constructor(s)
        public InputSetting()
        {
            InitializeComponent();
            DataContext = this;
            ControlText = _defaultControlText;

            // Inject ninject fields
            NinjectHelper.Kernel.Inject(this);
        }

        // Event Handlers
        public void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isWaitingForInput && MainWindow.InputsEnabled)
            {
                _isWaitingForInput = true;

                ControlText = "Select key..";

                // listen for mouse and keyboard events
                _hookManager.Hook.MousePressed += OnMousePressed;
                _hookManager.Hook.KeyPressed += OnKeyPressed;

                if (_hookManager.Hook !=  null && !_hookManager.Hook.IsRunning)
                {
                    _hookManager.Hook.RunAsync();
                }
            }
        }

        public void OnMousePressed(object sender, MouseHookEventArgs e)
        {
            HandleInputReceived(InputType.Mouse, e.Data.Button.ToString());
        }

        public void OnKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            HandleInputReceived(InputType.Keyboard, e.Data.KeyCode.ToString());
        }

        // When input is received, update control text and update lock
        private void HandleInputReceived(InputType inputType, string inputValue)
        {
            if (_isWaitingForInput)
            {
                // Unhook event handlers for mouse and key input
                _hookManager.Hook.MousePressed -= OnMousePressed;
                _hookManager.Hook.KeyPressed -= OnKeyPressed;

                string oldControlText = ControlText;
                // Update internal input value and text
                if (inputType == InputType.Keyboard)
                {
                    ControlText = inputValue[2..];
                } else
                {
                    ControlText = inputValue;
                }

                // Set clicker hotkey with new input value
                try
                {
                    ClickerSettings.Hotkey = new Hotkey(inputValue);
                } catch (InputNotFoundException)
                {
                    Trace.WriteLine("Unknown input value provided, reverting control text");
                    ControlText = oldControlText;
                }
                

                // Signal that we are no longer waiting for input
                _isWaitingForInput = false;
            }
        }

    }
}
