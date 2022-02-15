using Caliburn.Micro;
using MMNElectric.ViewModels;
using System.Windows;

namespace MMNElectric
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();

        }
    }
}
