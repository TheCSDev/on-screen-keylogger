using System.Reflection;
using System.Runtime.InteropServices;

namespace on_screen_keylogger
{
    /// <summary>
    /// Contains important constants such as constant global settings.
    /// </summary>
    public static class Const
    {
        //========================================================
        /// <summary>
        /// The UUID that is used for this application.
        /// Returns the GUID if there is one assigned,
        /// returns null otherwise.
        /// </summary>
        public static string AppGUID
        {
            get
            {
                GuidAttribute guid = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>();
                if (guid != null) return guid.Value;
                else return null;
            }
        }
        //--------------------------------------------------------
        /// <summary>
        /// The prefix that must be used by web messages in order
        /// for them to be processed.
        /// </summary>
        public static string WebMessagePrefix => "OnScreenKeylogger: ".ToLower();

        public static string WebMsg_LoadLayout => "LoadLayout".ToLower();
        public static string WebMsg_Reload => "reload".ToLower();
        public static string WebMsg_GoBack => "GoBack".ToLower();
        public static string WebMsg_PressKey => "PressKey".ToLower();
        public static string WebMsg_ReRegisterKeys => "ReRegisterKeys".ToLower();
        public static string WebMsg_MouseMove => "MouseMove".ToLower();
        public static string WebMsg_SetWindowSize => "SetWindowSize".ToLower();
        public static string WebMsg_Set => "set".ToLower();
        //========================================================
        /// <summary>
        /// This HTML attribute is assigned to all HTML elements
        /// that wish to be assigned the appropriate key press
        /// and release classes.
        /// </summary>
        public const string Attr_KeyCode = "keyCode";

        public const string Attr_OnPressed = "onpressed";
        public const string Attr_OnReleased = "onreleased";
        //--------------------------------------------------------
        /// <summary>
        /// This class is assigned automatically to all HTML
        /// elements with the <see cref="Attr_KeyCode"/>
        /// attribute when their key is pressed.
        /// </summary>
        public const string Class_PrsKey = "pressedKey";

        /// <summary>
        /// This class is assigned automatically to all HTML
        /// elements with the <see cref="Attr_KeyCode"/>
        /// attribute when their key is released.
        /// </summary>
        public const string Class_RelKey = "releasedKey";
        //--------------------------------------------------------
        /// <summary>
        /// This ID is assigned to all textual HTML elements
        /// whose innerHTML will be set to the value of
        /// <see cref="UpdateCallers.UpdateCaller.UpdateFrequency"/>.
        /// </summary>
        public const string Id_HzCounter = "updateFrequencyCounter";
        //========================================================
    }
}
