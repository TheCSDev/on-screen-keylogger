using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using on_screen_keylogger.Input;
using on_screen_keylogger.UpdateCallers;
using System.Collections.Generic;

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
        private readonly UpdateCaller _updateCaller;
        private readonly HashSet<string> _loadedKeyCodes = new HashSet<string>();
        //--------------------------------------------------------
        /// <summary>
        /// The <see cref="InputHandler"/> used by this window.<br/>
        /// Override this to define your own <see cref="InputHandler"/>.
        /// </summary>
        public InputHandler InptHandler => _inputHandler ??= new DefaultInputHandler();
        
        /// <summary>
        /// Returns the <see cref="UpdateCaller"/> used by this window.
        /// </summary>
        public UpdateCaller UpdateCaller => _updateCaller;

        /// <summary>
        /// The currently selected UI layout name.
        /// </summary>
        public string HtmlUILayoutName
        {
            get => this[Const.Setting_UILayoutName].ToString() ?? "default";
            set { this[Const.Setting_UILayoutName] = value; LoadHtmlLayout(); }
        }
        //========================================================
        public MainWindow()
        {
            //init ui
            InitializeComponent();
            LoadHtmlLayout();

            //define the task
            _updateCaller = new DefaultUpdateCaller(this);

            //update ui
            UpdateHtmlUI();
        }
        //--------------------------------------------------------
        /// <summary>
        /// Loads an HTML template layout for the UI.
        /// </summary>
        /// <returns>
        /// True if the layout got loaded, and false if something
        /// went wrong while loading.
        /// </returns>
        public bool LoadHtmlLayout(string layoutName = null)
        {
            layoutName ??= HtmlUILayoutName;
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
        public void UpdateHtmlUI()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                //load and focus check
                if (!webBrowser.IsLoaded || webBrowser.CoreWebView2 == null || IsFocused) return;
                
                //----- update html
                //update counters
                await ExecJS_ForEachQueryAsync(
                    "[id="+Const.Id_HzCounter+"]",
                    "i.innerHTML = '" + UpdateCaller.UpdateFrequency + "'");

                //update keycodes
                foreach(string keyCode in _loadedKeyCodes)
                {
                    //check if pressed and define className based on pressed
                    bool pressed = InptHandler.IsKeyDown(keyCode);
                    string className = pressed ? Const.Class_PrsKey : Const.Class_RelKey;

                    //iterate all elements with the keyCode=keyCode attribute,
                    //and add the appropriate class to it, and remve opposite class
                    string query = "[{$attrKeyCode}={$keyCode}]"
                        .Replace("{$attrKeyCode}", Const.Attr_KeyCode)
                        .Replace("{$keyCode}", keyCode);

                    //'var removed' is used to prevent the release event from being
                    //called right at the start during the first update
                    string js =
                    ("var removed = false;" +
                    "if(i.classList.contains('{$a}')) { i.classList.remove('{$a}'); removed = true; }" +
                    "if(!i.classList.contains('{$b}'))" +
                    "{" +
                    "    i.classList.add('{$b}');" +
                    "    if(i.hasAttribute('{$c}') && removed)" +
                    "        setTimeout(() => eval(i.getAttribute('{$c}')), 0);" +
                    "}");

                    if (pressed)
                    {
                        js = js.Replace("{$a}", Const.Class_RelKey).Replace("{$b}", Const.Class_PrsKey)
                            .Replace("{$c}", Const.Attr_OnPressed);
                    }
                    else
                    {
                        js = js.Replace("{$b}", Const.Class_RelKey).Replace("{$a}", Const.Class_PrsKey)
                            .Replace("{$c}", Const.Attr_OnReleased);
                    }

                    await ExecJS_ForEachQueryAsync(query, js);
                }
            });
        }
        //--------------------------------------------------------
        /// <summary>
        /// Retrieves the set of keyCode-s used by the currently
        /// loaded webpage in the <see cref="webBrowser"/>.<br/>
        /// Please note that this method waits for async tasks and may pause the thread.
        /// </summary>
        private async Task UpdateKeyCodesAsync()
        {
            //clear old stuff
            Console.WriteLine("[UpdateKeyCodesAsync] Clearing old keyCodes");
            _loadedKeyCodes.Clear();
            
            //wait and hope it will load by then cuz webBrowser.IsLoaded
            //is lying most of the time for some reason
            while (webBrowser.CoreWebView2 == null || !webBrowser.IsLoaded)
                await Task.Delay(100);
            await Task.Delay(300);

            string js =
            "(function(){\n" +
            "    var arr = [];\n" +
            "    document.querySelectorAll('["+Const.Attr_KeyCode+"]')" +
            ".forEach(i => arr.push(i.getAttribute('"+Const.Attr_KeyCode+"')));\n" +
            "    return arr.join(',')\n" +
            "})();";

            Task task = ExecJSAsync(js).ContinueWith((arg) =>
            {
                //gotta love the fast that JS adds quotes at the start and end /j
                string result = arg.Result.Replace("\"", "");
                Console.WriteLine("[UpdateKeyCodesAsync] Registering new keyCodes: " + result);
                _loadedKeyCodes.Clear();
                _loadedKeyCodes.UnionWith(result.Split(','));
            });

            await task;
        }
        //--------------------------------------------------------
        /// <summary>
        /// Executes JS on the <see cref="webBrowser"/>.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// When the <see cref="webBrowser"/>'s CoreWebView2 is null.
        /// </exception>
        public Task<string> ExecJSAsync(string js) =>
            webBrowser.CoreWebView2.ExecuteScriptAsync(js);
        //
        /// <summary>
        /// Executes the script:<br/>
        /// <b>document.querySelectorAll("selector").forEach(i => { iAction });</b><br/>
        /// on the <see cref="webBrowser"/>.
        /// </summary>
        public Task<string> ExecJS_ForEachQueryAsync(string selector, string iAction) =>
            ExecJSAsync("document.querySelectorAll(\"" + selector + "\")" +
                ".forEach(i => { " + iAction + " });");
        //========================================================
        /// <summary>
        /// This menu item closes the window when clicked.
        /// </summary>
        private void mf_exit_Click(object sender, RoutedEventArgs e) => Close();
        //--------------------------------------------------------
        /// <summary>
        /// Aborts the updater thread upon closing, and clears all data.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //dispose and clear stuff
            UpdateCaller?.Dispose();
            webBrowser?.CoreWebView2?.CookieManager?.DeleteAllCookies();

            //save properties
            Properties.Settings.Default.Save();
        }
        //--------------------------------------------------------
        /// <summary>
        /// Updates <see cref="_loadedKeyCodes"/>.
        /// </summary>
        private async void webBrowser_NavigationCompleted
        (object sender, CoreWebView2NavigationCompletedEventArgs e)
            => await UpdateKeyCodesAsync();
        //--------------------------------------------------------
        /// <summary>
        /// For security reasons, deny all navigations to websites.
        /// One security example is having a local UI layout file
        /// suddenly redirect the browser to a malicious website.
        /// Stuff like that must not happen.
        /// </summary>
        private void webBrowser_NavigationStarting
        (object sender, CoreWebView2NavigationStartingEventArgs e) =>
            e.Cancel = !new Uri(e.Uri).IsFile;
        //--------------------------------------------------------
        /// <summary>
        /// Define CoreWebView2 settings.
        /// </summary>
        private void webBrowser_CoreWebView2InitializationCompleted
        (object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webBrowser.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
            webBrowser.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
        }
        //========================================================
        /// <summary>
        /// Gets or sets the application's user settings.
        /// </summary>
        /// <param name="setting">The name of the setting.</param>
        public object this[string setting]
        {
            get => GetSetting(setting);
            set => SetSetting(setting, value);
        }
        //--------------------------------------------------------
        public static T GetSetting<T>(string setting)
        {
            try { return (T)Properties.Settings.Default[setting]; }
            catch { return default; }
        }
        //
        public static object GetSetting(string setting)
        {
            try { return Properties.Settings.Default[setting]; }
            catch { return null; }
        }
        //--------------------------------------------------------
        public static void SetSetting(string setting, object value) =>
            Properties.Settings.Default[setting] = value;
        //========================================================
    }
}
