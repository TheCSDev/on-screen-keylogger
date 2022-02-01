using System.Windows;
using on_screen_keylogger.Properties;

namespace on_screen_keylogger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //========================================================
        /// <summary>
        /// Tracks the startup args.
        /// </summary>
        public static string[] StartupArgs { get; private set; }
        //--------------------------------------------------------
        /// <summary>
        /// The startup layout.
        /// </summary>
        public static string StartupLayout { get; private set; } = Settings.Default.UILayoutName;
        //========================================================
        protected override void OnStartup(StartupEventArgs e)
        {
            StartupArgs = e.Args;
            StartupLayout = string.Join(" ", StartupArgs); //may likely change
            base.OnStartup(e);
        }
        //========================================================
    }
}
