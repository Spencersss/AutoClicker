using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace SpencerAutoClicker.Source.Frontend.Controls
{


    /// <summary>
    /// Interaction logic for HotkeySetting.xaml
    /// </summary>
    public partial class InputSetting : UserControl
    {
        // Vars
        private const string _defaultControlText = "Set key..";
        public string ControlText { get; set; } = _defaultControlText;

        // State vars
        private bool _isSelectingInput { get; set; } = false;

        // Constructor(s)
        public InputSetting()
        {
            InitializeComponent();
        }

        // Event Handlers
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            /*
            ControlText = _isSelectingInput ?  "." : _defaultControlText;
            _isSelectingInput = !_isSelectingInput;
            */
        }
        
    }
}
