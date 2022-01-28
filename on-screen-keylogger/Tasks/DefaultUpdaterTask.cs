using System;

namespace on_screen_keylogger.Tasks
{
    /// <summary>
    /// The default <see cref="UpdaterTask"/> used by this app.
    /// </summary>
    public sealed class DefaultUpdaterTask : UpdaterTask
    {
        //========================================================
        /// <summary>
        /// The window that should be updated on upate.
        /// </summary>
        public readonly MainWindow ParentWindow;
        //========================================================
        public DefaultUpdaterTask(MainWindow parent) => ParentWindow = parent;
        //--------------------------------------------------------
        public override void OnUpdate() => ParentWindow?.UpdateUI();
        //========================================================
    }
}
