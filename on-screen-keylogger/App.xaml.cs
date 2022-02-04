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
        public static string StartupLayout { get; private set; } = "default";
        //========================================================
        protected override void OnStartup(StartupEventArgs e)
        {
            StartupArgs = e.Args;

            string layout = string.Join(" ", StartupArgs);
            if (string.IsNullOrWhiteSpace(layout)) layout = Settings.Default.UILayoutName;
            
            StartupLayout = layout; //may likely change
            base.OnStartup(e);
        }
        //========================================================
    }
}
