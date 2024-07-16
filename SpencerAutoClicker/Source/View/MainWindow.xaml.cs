using Ninject;
using SpencerAutoClicker.Source.Frontend.Controls;
using SpencerAutoClicker.Source.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SpencerAutoClicker.Source.Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Clicker button texts
        private const string START_CLICKER_BASE_STRING = "Start Clicker ({0})";
        private const string STOP_CLICKER_BASE_STRING = "Stop Clicker ({0})";

        // Colors
        public readonly SolidColorBrush StartColor =
            new SolidColorBrush(Color.FromRgb(123, 237, 159));
        public readonly SolidColorBrush StopColor =
            new SolidColorBrush(Color.FromRgb(255, 107, 129));

        // Vars
        public static SortedDictionary<string, Process> Apps;
        private readonly Clicker _clicker;
        private readonly HookManager _hotkeyHook;

        // State vars
        public static bool InputsEnabled { get; set; }

        // Constructor
        public MainWindow(HookManager hookManager, IKernel kernel)
        {
            DataContext = this;
            InitializeComponent();

            _hotkeyHook = hookManager;
            hotkeyConfig = kernel.Get<InputSetting>();
            hotkeyConfig.Visibility = Visibility.Visible;

            Apps = new SortedDictionary<string, Process>();

            // init state vars
            InputsEnabled = true;

            // Init button text
            clicker_button.Content = string.Format(START_CLICKER_BASE_STRING, ClickerSettings.Hotkey.ToString());

            // Init private vars
            _clicker = new Clicker();

            // Setup event handlers
            _hotkeyHook.OnHotkeyTriggered += OnHotkeyTriggered;
            ClickerSettings.OnHotkeyChanged += OnHotkeyChanged;
        }

        // Helper for populating processes
        private void AddProcess(Process proc)
        {
            if ((int)proc.MainWindowHandle != 0 && proc.MainWindowTitle.Length > 0
                && !Apps.ContainsKey(proc.MainWindowTitle))
            {
                Apps.Add(proc.MainWindowTitle, proc);
            }
        }

        // Methods
        public InputSetting GetHotkeyConfig()
        {
            return hotkeyConfig;
        }

        private void PopulateProcesses()
        {
            // Populate Processes
            Apps = new SortedDictionary<string, Process>();
            List<Process> processes = Process.GetProcesses().ToList();
            foreach (Process proc in processes)
            {
                AddProcess(proc);
            }
        }

        // Setup one way data binding
        private void SetupProcessDataBinding()
        {
            process_list.ItemsSource = Apps;
            process_list.SelectedValuePath = "Value.MainWindowTitle";
            process_list.DisplayMemberPath = "Value.MainWindowTitle";
        }

        // Returns whether or not a key is numeric
        public bool IsDigit(Key key)
        {
            int keyVal = (int)key;
            return keyVal > 33 && keyVal < 44 || keyVal > 73 && keyVal < 84;
        }

        // If bad input entered for interval, set to minimum value.
        private void VerifyInterval()
        {
            if (click_interval.Text.Length <= 0)
            {
                click_interval.Text = "20";
                ClickerSettings.ClickInterval = 20;
            }
            else
            {
                int currentInterval = int.Parse(click_interval.Text);
                if (click_interval.Text.Length <= 2 && currentInterval < 20)
                {
                    click_interval.Text = "20";
                    ClickerSettings.ClickInterval = 20;
                }
            }
        }

        // true = show 'start clicker', false = show 'stop clicker'
        private void SetClickerButtonState(bool state)
        {
            Dispatcher.Invoke(() =>
            {
                if (state)
                {
                    clicker_button.Content = string.Format(START_CLICKER_BASE_STRING, ClickerSettings.Hotkey.ToString());
                    clicker_button.Background = StartColor;
                }
                else
                {
                    clicker_button.Content = string.Format(STOP_CLICKER_BASE_STRING, ClickerSettings.Hotkey.ToString());
                    clicker_button.Background = StopColor;
                }
            });
        }

        private void SetInputEnabled(bool enabled)
        {
            InputsEnabled = enabled;
            click_interval.IsReadOnly = !enabled;
            hold_mode.IsEnabled = enabled;
        }

        // Event Handlers
        private void Process_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0].GetType() == typeof(KeyValuePair<string, Process>))
                {
                    KeyValuePair<string, Process> addedItem = (KeyValuePair<string, Process>)e.AddedItems[0];
                    _clicker.SetProcess(addedItem.Value);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateProcesses();
            SetupProcessDataBinding();
            click_interval.Text = ClickerSettings.ClickInterval.ToString();
        }

        // Cleanup when clicker window is closed
        private void OnWindowClose(object sender, EventArgs e)
        {
            _hotkeyHook.CleanupActiveHooks();
            ClickerSettings.OnHotkeyChanged -= OnHotkeyChanged;
            Environment.Exit(Environment.ExitCode);
        }

        private void OnHotkeyChanged(object sender, string newHotkeyString)
        {
            SetClickerButtonState(!_clicker.ClickerRunning);
        }

        private void HandleClickerAuto()
        {
            if (_clicker.ClickerRunning)
            {
                SetInputEnabled(true);
                SetClickerButtonState(true);
                _clicker.StopClicker();
            }
            else
            {
                if (_clicker.IsProcessSelected())
                {
                    SetInputEnabled(false);
                    SetClickerButtonState(false);
                    _clicker.StartClicker();
                }
            }
        }

        private void OnHotkeyTriggered(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                VerifyInterval();
                HandleClickerAuto();
            }));
        }

        private void OnEnabledHoldMode(object sender, RoutedEventArgs _)
        {
            ClickerSettings.ShouldHoldDown = true;
        }

         private void OnDisableHoldMode(object sender, RoutedEventArgs _)
        {
            ClickerSettings.ShouldHoldDown = false;
        }

        private void Click_Interval_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (click_interval.Text.Length > 0)
            {
                int newClickInterval = int.Parse(click_interval.Text);
                ClickerSettings.ClickInterval = newClickInterval;
            }
        }

        private void Click_Interval_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Back)
            {
                if (IsDigit(e.Key))
                {
                    if (click_interval.Text.Length > 0)
                    {
                        int newClickInterval = int.Parse(click_interval.Text);
                        e.Handled = newClickInterval > int.MaxValue;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void Process_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!process_list.IsDropDownOpen)
            {
                PopulateProcesses();
                SetupProcessDataBinding();
            }
        }
        
    }
}
