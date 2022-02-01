using System.Runtime.InteropServices;

namespace on_screen_keylogger.Handlers
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
        public DefaultInputHandler(MainWindow parent) : base(parent) { }
        //========================================================
        /// <summary>
        /// This overriden method uses <see cref="GetKeyState(int)"/>
        /// from User32.dll to check if a key is down.
        /// </summary>
        public override bool IsKeyDown(string key) =>
            (GetKeyState(ParseKeyCode(key) ?? 0) & 0x8000) != 0;
        //========================================================
    }
}
