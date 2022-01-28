using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace on_screen_keylogger.Input
{
    /// <summary>
    /// The default built-in <see cref="InputHandler"/>.
    /// It only supports keyboard keyCodes.
    /// </summary>
    public sealed class DefaultInputHandler : InputHandler
    {
        //========================================================
        /// <summary>
        /// SHORT (Winuser.h).GetKeyState(int)
        /// </summary>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);
        //========================================================
        /// <summary>
        /// This overriden method uses <see cref="GetKeyState(int)"/>
        /// from User32.dll to check if a key is down.
        /// </summary>
        public override bool IsKeyDown(string key)
        {
            //get keyCode
            int keyCode;
            if (!int.TryParse(key, out keyCode)) //try int
            {
                Keys keyEnum;
                if(!Enum.TryParse(key, true, out keyEnum)) //then try keys enum
                    return false;
                keyCode = (int)keyEnum;
            }

            //return
            return (GetKeyState(keyCode) & 0x8000) != 0;
        }
        //========================================================
    }
}
