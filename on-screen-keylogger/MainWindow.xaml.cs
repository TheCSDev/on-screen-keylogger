using mshtml;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Controls;
using on_screen_keylogger.Input;
using on_screen_keylogger.Tasks;

namespace on_screen_keylogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //========================================================
        protected InputHandler _inputHandler;
        //
        private readonly UpdaterTask _updaterTask;
        //--------------------------------------------------------
        /// <summary>
        /// The <see cref="InputHandler"/> used by this window.<br/>
        /// Override this to define your own <see cref="InputHandler"/>.
        /// </summary>
        public InputHandler InptHandler => _inputHandler ??= new DefaultInputHandler();

        /// <summary>
        /// Returns the <see cref="UpdaterTask"/> used by this window.
        /// </summary>
        public UpdaterTask UpdaterTask => _updaterTask;

        /// <summary>
        /// The currently selected UI layout name.
        /// </summary>
        public string UILayoutName
        {
            get => ConfigurationManager.AppSettings["UILayoutName"];
            set
            {
                SetAppSetting("UILayoutName", value);
                LoadUILayout(value);
            }
        }
        //========================================================
        public MainWindow()
        {
            //init ui
            InitializeComponent();
            LoadUILayout();

            //define the task
            _updaterTask = new DefaultUpdaterTask(this);

            //update ui
            UpdateUI();
        }
        //--------------------------------------------------------
        /// <summary>
        /// Loads an HTML template layout for the UI.
        /// </summary>
        /// <returns>
        /// True if the layout got loaded, and false if something
        /// went wrong while loading.
        /// </returns>
        public bool LoadUILayout(string layoutName = null)
        {
            layoutName ??= UILayoutName;
            try
            {
                string path = string.Format("./layouts/{0}/layout.html", layoutName);
                FileInfo file = new FileInfo(path);
                if(!file.Exists) throw new FileNotFoundException();
                webBrowser.Source = new Uri(file.FullName);
                return true;
            }
            catch { return false; }
        }
        //--------------------------------------------------------
        /// <summary>
        /// Updates all of the UI elements, their texts, and colors.
        /// </summary>
        public void UpdateUI()
        {
            Dispatcher.Invoke(() =>
            {
                if (!webBrowser.IsLoaded || IsFocused) return;
                //get html document
                HTMLDocument document = webBrowser.Document as HTMLDocument;
                if(document == null) return;

                //update html
                foreach (IHTMLElement element in document.getElementsByTagName("*"))
                    try
                    {
                        //update frequency counter
                        if (element.id?.Contains("updateFrequencyCounter") ?? false)
                            element.innerHTML = UpdaterTask.UpdateFrequency.ToString();

                        //update tag based on key press
                        string keyCode = Convert.ToString(element.getAttribute("keyCode"));
                        if (!string.IsNullOrEmpty(keyCode))
                        {
                            bool pressed = InptHandler.IsKeyDown(keyCode);
                            if (pressed && !("" + element.className).Contains("pressedKey"))
                            {
                                string className = element.className + " pressedKey";
                                className = className.Replace("releasedKey", "").Trim();
                                element.className = className;
                            }
                            else if(!pressed && !("" + element.className).Contains("releasedKey"))
                            {
                                string className = element.className + " releasedKey";
                                className = className.Replace("pressedKey", "").Trim();
                                element.className = className;
                            }
                        }
                    }
                    catch { }
            });
        }
        //--------------------------------------------------------
        /// <summary>
        /// A protected call for <see cref="WebBrowser.InvokeScript(string, object[])"/>
        /// that is safe from thrown exceptions.
        /// </summary>
        public bool PCallJS(string funcName, params object[] args)
        {
            try { webBrowser.InvokeScript(funcName, args); return true; }
            catch { return false; }
        }
        //========================================================
        /// <summary>
        /// This menu item closes the window when clicked.
        /// </summary>
        private void mf_exit_Click(object sender, RoutedEventArgs e) => Close();
        //--------------------------------------------------------
        /// <summary>
        /// Aborts the updater thread upon closing.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) =>
            UpdaterTask?.Dispose();
        //--------------------------------------------------------
        /// <summary>
        /// And this one is just... stupid (in terms of performance).
        /// This method prevents using backspace to go back cuz apparently
        /// you cannot disable navigation for WebBrowser-s.
        /// </summary>
        private void webBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            StackTrace st = new StackTrace();
            if (!st.GetFrames().Any(i => i.GetMethod().Name.Equals(nameof(LoadUILayout))))
                e.Cancel = true;
        }
        //========================================================
        /// <summary>
        /// (Copy/Paste)d from:<br/>
        /// https://stackoverflow.com/questions/5274829/configurationmanager-appsettings-how-to-modify-and-save
        /// </summary>
        public static bool SetAppSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null) { settings.Add(key, value); }
                else { settings[key].Value = value; }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
                return false;
            }
        }
        //========================================================
    }
}
