using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SpencerAutoClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Colors
        public readonly SolidColorBrush StartColor = 
            new SolidColorBrush(Color.FromRgb(123, 237, 159));
        public readonly SolidColorBrush StopColor =
            new SolidColorBrush(Color.FromRgb(255, 107, 129));

        // Fields
        public static SortedDictionary<string, Process> Apps;
        private Clicker _clicker;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            Apps = new SortedDictionary<string, Process>();
            _clicker = new Clicker();

            // Init button text
            clicker_button.Content = "Start Clicker (" + _clicker.Hotkey_Mouse_Click + ")";
        }

        // Helper for populating processes
        private void addProcess(Process proc)
        {
            if ((int)proc.MainWindowHandle != 0 && proc.MainWindowTitle.Length > 0
                && !Apps.ContainsKey(proc.MainWindowTitle))
            {
                Apps.Add(proc.MainWindowTitle, proc);
            }
        }

        // Methods
        private void populateProcesses()
        {
            // Populate Processes
            Apps = new SortedDictionary<string, Process>();
            List<Process> processes = Enumerable.ToList(Process.GetProcesses());
            foreach (Process proc in processes)
            {
                addProcess(proc);
            }
        }

        private void setupProcessDataBinding()
        {
            // Setup one way data binding
            process_list.ItemsSource = Apps;
            process_list.SelectedValuePath = "Value.MainWindowTitle";
            process_list.DisplayMemberPath = "Value.MainWindowTitle";
        }

        // Returns whether or not a key is numeric
        public bool IsDigit(Key key)
        {
            int keyVal = (int)key;
            return (keyVal > 33 && keyVal < 44) || (keyVal > 73 && keyVal < 84);
        }

        // Event Handlers
        private void Process_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0].GetType() == typeof(KeyValuePair<int, Process>))
                {
                    KeyValuePair<int, Process> addedItem = (KeyValuePair<int, Process>)e.AddedItems[0];
                    _clicker.SetProcess(addedItem.Value);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            populateProcesses();
            setupProcessDataBinding();
        }

        private void Clicker_Handler(object sender, RoutedEventArgs e)
        {
            if (_clicker.ClickerRunning)
            {
                _clicker.StopClicker();
                clicker_button.Content = "Start Clicker (" + _clicker.Hotkey_Mouse_Click + ")";
                clicker_button.Background = StartColor;
            }
            else
            {
                if (_clicker.IsProcessSelected())
                {
                    _clicker.StartClicker();
                    clicker_button.Content = "Stop Clicker (" + _clicker.Hotkey_Mouse_Click + ")";
                    clicker_button.Background = StopColor;
                }
            }
        }

        private void Click_Interval_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (click_interval.Text.Length > 0)
            {
                int newClickInterval = int.Parse(click_interval.Text);
                _clicker.ClickInterval = newClickInterval;
            }
        }

        private void Click_Interval_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !IsDigit(e.Key) || ((click_interval.Text.Length + 1) > 9);
        }

        private void Process_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!process_list.IsDropDownOpen)
            {
                populateProcesses();
                setupProcessDataBinding();
            }
        }
    }
}
