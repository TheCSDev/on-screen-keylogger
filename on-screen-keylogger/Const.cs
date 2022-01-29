﻿using System;
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
        /// This <see cref="Properties.Settings"/> setting tells the
        /// application what HTML file to use as the UI layout.
        /// </summary>
        public const string Setting_UILayoutName = "UILayoutName";

        /// <summary>
        /// This <see cref="Properties.Settings"/> setting tells the
        /// application how long to wait before calling another update.
        /// </summary>
        public const string Setting_UpdateTimeMS = "UpdateTimeMS";
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
        //Also some extension methods cuz why not
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        //========================================================
    }
}