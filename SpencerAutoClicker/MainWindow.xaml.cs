using System;
using System.Collections.Generic;
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
        // Vars
        public static Dictionary<int, Process> Apps;

        public MainWindow()
        {
            InitializeComponent();
            Apps = new Dictionary<int, Process>();
        }

        // Event Handlers
        private void Window_Loaded(object s, RoutedEventArgs e)
        {
            List<Process> processes = Enumerable.ToList(Process.GetProcesses());
            foreach (Process proc in processes)
            {
                if ((int)proc.MainWindowHandle != 0)
                {
                    Apps.Add(proc.Id, proc);
                }   
            }
        }

        private void Process_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine(e.AddedItems[0]);
        }
    }
}
