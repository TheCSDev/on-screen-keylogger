namespace on_screen_keylogger.Input
{
    /// <summary>
    /// This class handles user input. It is used to tell if
    /// a specific key is currently pressed or not.
    /// </summary>
    public abstract class InputHandler
    {
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
        //========================================================
    }
}
