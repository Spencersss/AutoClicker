using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using Ninject.Parameters;
using SpencerAutoClicker.Source.Backend;
using SpencerAutoClicker.Source.Backend.Helpers;
using SpencerAutoClicker.Source.Frontend;
using SpencerAutoClicker.Source.Frontend.Controls;

namespace SpencerAutoClicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Vars
        private IKernel _kernel;

        // Event handlers
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            _kernel = NinjectHelper.Kernel;

            // Bindings
            _kernel.Bind<HookManager>().ToSelf().InSingletonScope();
            _kernel.Bind<MainWindow>().ToSelf();

            // Setup main window
            MainWindow = _kernel.Get<MainWindow>(new ConstructorArgument("kernel", _kernel));
            MainWindow.Show();
        }
    }
}