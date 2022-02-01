using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace on_screen_keylogger.Handlers
{
    /// <summary>
    /// This class handles user input. It is used to tell if
    /// a specific key is currently pressed or not.
    /// </summary>
    public abstract class InputHandler
    {
        //========================================================
        /// <summary>
        /// The window that this input handler belongs to.
        /// </summary>
        public readonly MainWindow Parent;
        //--------------------------------------------------------
        /// <summary>
        /// Simulates input.
        /// </summary>
        public readonly InputSimulator inputSimulator;
        //--------------------------------------------------------
        /// <summary>
        /// Returns true while a key is being pressed.
        /// While this is true, <see cref="OSK_KeyPress(int)"/>
        /// cannot be called.
        /// </summary>
        public bool AKeyIsPressing { get; private set; } = false;
        //========================================================
        public InputHandler(MainWindow parent) =>
            (Parent, inputSimulator) = (parent, new InputSimulator());
        //========================================================
        /// <summary>
        /// Returns true if a <paramref name="key"/> is down.
        /// The parameter is a string so as to allow passing
        /// custom identifiers for keys such as key codes.
        /// </summary>
        public abstract bool IsKeyDown(string key);
        //--------------------------------------------------------
        /// <summary>
        /// Returns true if a <paramref name="keyCode"/> is down.
        /// </summary>
        public virtual bool IsKeyCodeDown(int keyCode) => IsKeyDown(keyCode.ToString());
        //--------------------------------------------------------
        /// <summary>
        /// Convert a key code string to int.
        /// </summary>
        public virtual int? ParseKeyCode(string key)
        {
            int keyCode;
            if (!int.TryParse(key.Trim(), out keyCode)) //try int
            {
                Keys keyEnum;
                if (!Enum.TryParse(key, true, out keyEnum)) //then try keys enum
                    return null;
                keyCode = (int)keyEnum;
            }
            return keyCode;
        }
        //========================================================
        /// <summary>
        /// Simulates an On-Screen Keyboard key press
        /// </summary>
        public virtual void OSK_KeyPress(int keyCode)
        {
            //null check
            if (keyCode < 1) return;

            //check if a key is already being pressed
            if (AKeyIsPressing) return;
            AKeyIsPressing = true;

            //redirect focus onto the last focused window
            IntPtr hWnd = Parent.LastActiveWindow;
            if (hWnd == null || hWnd == IntPtr.Zero) return;

            Focus(hWnd);
            Thread.Sleep(25); //wait a bit for the focus to take place

            //press the key
            try { inputSimulator.Keyboard.KeyPress((VirtualKeyCode)keyCode); }
            catch { }

            //reset AKeyIsPressing
            AKeyIsPressing = false;
        }
        //========================================================
        private void Focus(IntPtr hWnd) { Utils.SetForegroundWindow(hWnd); Utils.SetFocus(hWnd); }
        //========================================================
    }
}
