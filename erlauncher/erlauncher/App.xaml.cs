using System.Windows;

namespace erlauncher
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the App class.
        /// Sets ShutdownMode to OnMainWindowClose.
        /// </summary>
        public App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}