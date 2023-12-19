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
using SharpHook;
using SharpHook.Native;

namespace SpencerAutoClicker.Source.Frontend.Controls
{


    /// <summary>
    /// Interaction logic for HotkeySetting.xaml
    /// </summary>
    public partial class InputSetting : UserControl, INotifyPropertyChanged
    {
        // Enums
        private enum InputType
        {
            Mouse,
            Keyboard
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Vars
        private SimpleGlobalHook hook;
        private const string _defaultControlText = "Set";
        private string _controlText = "";
        private string _currentInput = "";

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
            hook = new SimpleGlobalHook();
        }

        // Event Handlers
        public void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isWaitingForInput)
            {
                _isWaitingForInput = true;

                ControlText = "Select key..";

                // listen for mouse and keyboard events
                hook.MousePressed += OnMousePressed;
                hook.KeyPressed += OnKeyPressed;

                if (hook !=  null && !hook.IsRunning)
                {
                    hook.RunAsync();
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
                hook.MousePressed -= OnMousePressed;
                hook.KeyPressed -= OnKeyPressed;

                
                // Update internal input value and text
                if (inputType == InputType.Keyboard)
                {
                    ControlText = inputValue[2..];
                } else
                {
                    ControlText = inputValue;
                }
                _currentInput = inputValue;

                // Signal that we are no longer waiting for input
                _isWaitingForInput = false;
            }
        }

    }
}
