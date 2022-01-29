using System;

namespace on_screen_keylogger.UpdateCallers
{
    /// <summary>
    /// The default <see cref="UpdateCaller"/> used by this app.
    /// </summary>
    public sealed class DefaultUpdateCaller : UpdateCaller
    {
        //========================================================
        /// <summary>
        /// The window that should be updated on upate.
        /// </summary>
        public readonly MainWindow ParentWindow;
        //========================================================
        public DefaultUpdateCaller(MainWindow parent) => ParentWindow = parent;
        //--------------------------------------------------------
        public override void OnUpdate() => ParentWindow?.UpdateHtmlUI();
        //========================================================
    }
}
