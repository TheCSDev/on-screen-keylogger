using System;

namespace on_screen_keylogger.Handlers
{
    public abstract class WebMessageHandler
    {
        //========================================================
        /// <summary>
        /// The window that this handler belongs to.
        /// </summary>
        public readonly MainWindow Parent;
        //--------------------------------------------------------
        /// <summary>
        /// Used to track if a resize message has already been sent.
        /// Only one resize message is allowed to be sent by a layout.
        /// </summary>
        internal bool ResizeCalled = false;
        //========================================================
        public WebMessageHandler(MainWindow parent) =>
            Parent = parent ?? throw new ArgumentNullException("parent");
        //--------------------------------------------------------
        /// <summary>
        /// Handles the web message.
        /// </summary>
        public abstract void Handle(string message);
        //========================================================
    }
}
