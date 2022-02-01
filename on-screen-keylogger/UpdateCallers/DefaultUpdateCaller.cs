using System;

namespace on_screen_keylogger.UpdateCallers
{
    /// <summary>
    /// The default <see cref="UpdateCaller"/> used by this app.
    /// </summary>
    public sealed class DefaultUpdateCaller : UpdateCaller
    {
        //========================================================
        public DefaultUpdateCaller(MainWindow parent) : base(parent) { }
        //--------------------------------------------------------
        public override void OnUpdate() => Parent?.UpdateHtmlUI();
        //========================================================
    }
}
