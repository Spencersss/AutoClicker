using System.Windows;
using Ninject;
using Ninject.Parameters;
using SpencerAutoClicker.Source.Frontend;
using SpencerAutoClicker.Source.Model;
using SpencerAutoClicker.Source.Model.Helpers;

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