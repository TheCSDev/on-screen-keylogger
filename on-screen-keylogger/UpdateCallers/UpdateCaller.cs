using System;
using System.Threading;

namespace on_screen_keylogger.UpdateCallers
{
    /// <summary>
    /// This class runs a separate daemon <see cref="Thread"/>
    /// that calls <see cref="OnUpdate()"/> every <see cref="UpdateTime"/>
    /// milliseconds.
    /// </summary>
    public abstract class UpdateCaller : IDisposable
    {
        //========================================================
        protected int? _updateFrequency = null;
        //
        private readonly Thread _thread;
        //--------------------------------------------------------
        /// <summary>
        /// The amount of time to wait in milliseconds before
        /// updating the displayed keys. Lower value may cause more lag.<br/>
        /// Due to lag reasons, this number has been hard-coded to 25ms minimum.
        /// This number also cannot be greater than 250ms.
        /// </summary>
        public byte UpdateTime
        {
            get => (byte)MainWindow.GetSetting<int>(Const.Setting_UpdateTimeMS).Clamp(25, 250);
            set
            {
                MainWindow.SetSetting(Const.Setting_UpdateTimeMS, (int)value);
                _updateFrequency = null;
            }
        }

        /// <summary>
        /// How frequently is <see cref="OnUpdate()"/> gonna be called?
        /// </summary>
        public int UpdateFrequency => _updateFrequency ??= (1000 / UpdateTime);
        //========================================================
        public UpdateCaller()
        {
            _thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(UpdateTime);
                    OnUpdate();
                }
            });
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.BelowNormal;
            _thread.Start();
        }
        ~UpdateCaller() => Dispose();
        //--------------------------------------------------------
        /// <summary>
        /// This method is called every
        /// <see cref="UpdateTime"/> milliseconds.
        /// </summary>
        public abstract void OnUpdate();
        //--------------------------------------------------------
        /// <summary>
        /// Aborts the thread run by this <see cref="UpdateCaller"/>.
        /// </summary>
        public void Dispose() => _thread.Abort();
        //========================================================
    }
}
