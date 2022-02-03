using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace on_screen_keylogger.Handlers
{
    public sealed class DefaultWebMessageHandler : WebMessageHandler
    {
        //========================================================
        public DefaultWebMessageHandler(MainWindow parent) : base(parent) { }
        //--------------------------------------------------------
        private void LoadSettingsLayout()
        {
            //prepend settings data
            string uil = Parent.HtmlUILayoutName;
            int   utms = Parent.UpdateCaller.UpdateTime;
            bool   shm = Properties.Settings.Default.ShowMenu;
            bool   lrr = Properties.Settings.Default.LimitResizeRequests;

            string html =
                //Append settings variables
                "<!DOCTYPE html>\n" +
                "<script>" +
                "OnScreenKeylogger = {};" +
                "OnScreenKeylogger.Settings = {};" +
                "OnScreenKeylogger.Settings.UILayoutName        = \""+ uil +"\";" +
                "OnScreenKeylogger.Settings.UpdateTimeMS        = "  + utms +";" +
                "OnScreenKeylogger.Settings.ShowMenu            = "  + shm.ToString().ToLower() + ";" +
                "OnScreenKeylogger.Settings.LimitResizeRequests = "  + lrr.ToString().ToLower() + ";" +
                "</script>" +

                //Then the rest of the settings html
                Properties.Resources.layout_settings;

            //load page
            Parent.webBrowser.NavigateToString(html);
        }
        //========================================================
        public override void Handle(string message)
        {
            //log the message
            Console.WriteLine("[WebMessageReceived] " + message);

            //handle it
            Parent.Dispatcher.InvokeAsync(async () =>
            {
                //make sure the core is in place
                while (Parent.webBrowser.CoreWebView2 == null) await Task.Delay(100);

                //go back
                if (message.ToLower().StartsWith(Const.WebMsg_GoBack))
                {
                    if (Parent.webBrowser.CanGoBack) Parent.webBrowser.GoBack();
                    else Parent.LoadHtmlLayout(); //default
                }

                //load layout
                else if (message.ToLower().StartsWith(Const.WebMsg_LoadLayout + " "))
                {
                    //obtain layout name
                    message = message.Substring(Const.WebMsg_LoadLayout.Length + 1);

                    //handle
                    if (message.ToLower().Equals("default"))
                        Parent.webBrowser.NavigateToString(Properties.Resources.layout_default);
                    else if (message.ToLower().Equals("settings"))
                        LoadSettingsLayout();
                    else
                        Parent.LoadHtmlLayout(message);
                }

                //reload
                else if (message.ToLower().StartsWith(Const.WebMsg_Reload))
                    Parent.LoadHtmlLayout(); //re-loads layout

                //pressKey
                else if (message.ToLower().StartsWith(Const.WebMsg_PressKey + " "))
                {
                    message = message.Substring(Const.WebMsg_PressKey.Length + 1);
                    int? keyCode = Parent.InptHandler.ParseKeyCode(message);
                    Parent.InptHandler.OSK_KeyPress(keyCode ?? 0);
                }

                //ReRegisterKeys
                else if (message.ToLower().StartsWith(Const.WebMsg_ReRegisterKeys))
                    Parent.UpdateKeyCodes();

                //mouseMove
                else if (message.ToLower().StartsWith(Const.WebMsg_MouseMove + " "))
                    try
                    {
                        //substring, get cursor pos
                        message = message.Substring(Const.WebMsg_MouseMove.Length + 1);
                        Point cursorPos = Cursor.Position;

                        //get xy and relative pos info
                        string[] xy = message.Split(' ');
                        bool xRel = xy[0].StartsWith("~"), yRel = xy[1].StartsWith("~");

                        if (xRel) { xy[0] = xy[0].Substring(1); }
                        if (yRel) { xy[1] = xy[1].Substring(1); }

                        int x = int.Parse(xy[0]), y = int.Parse(xy[1]);

                        if (xRel) { x += cursorPos.X; }
                        if (yRel) { y += cursorPos.Y; }

                        //move
                        Cursor.Position = new Point(x, y);
                    }
                    catch { }

                //setWindowSize
                else if (message.ToLower().StartsWith(Const.WebMsg_SetWindowSize + " ") && !ResizeCalled)
                    try
                    {
                        ResizeCalled = true;

                        //substring, get width and height
                        message = message.Substring(Const.WebMsg_SetWindowSize.Length + 1);

                        //get xy and pos info
                        string[] xy = message.Split(' ');
                        int x = int.Parse(xy[0]), y = int.Parse(xy[1]);

                        //set size
                        Parent.Width = x;
                        Parent.Height = y;
                    }
                    catch { }

                //set (for security reasons, internal layouts only)
                else if (message.ToLower().StartsWith(Const.WebMsg_Set + " ") &&
                        Parent.IsCurrLayoutInternal)
                {
                    //get key/value
                    message = message.Substring(Const.WebMsg_Set.Length + 1);
                    Properties.Settings s = Properties.Settings.Default;

                    if (message.StartsWith(nameof(s.UILayoutName) + " "))
                    {
                        message = message.Substring(nameof(s.UILayoutName).Length + 1);
                        //Parent.HtmlUILayoutName = message; -- nope, this refreshes the page
                        s.UILayoutName = message; //now this... is fine
                    }
                    else if (message.StartsWith(nameof(s.UpdateTimeMS) + " "))
                    {
                        message = message.Substring(nameof(s.UpdateTimeMS).Length + 1);
                        byte num = 50;
                        byte.TryParse(message, out num);
                        Parent.UpdateCaller.UpdateTime = num;
                    }
                    else if (message.StartsWith(nameof(s.ShowMenu) + " "))
                    {
                        message = message.Substring(nameof(s.ShowMenu).Length + 1);
                        Parent.ShowMenu = ParseBoolButBetter(message);
                    }
                    else if (message.StartsWith(nameof(s.LimitResizeRequests) + " "))
                    {
                        message = message.Substring(nameof(s.LimitResizeRequests).Length + 1);
                        s.LimitResizeRequests = ParseBoolButBetter(message);
                    }
                }
            });
        }
        //========================================================
        /// <summary>
        /// A better version for <see cref="bool.Parse(string)"/>
        /// </summary>
        private bool ParseBoolButBetter(string input) =>
            input.Trim().ToLower().Equals("true");
        //========================================================
    }
}
