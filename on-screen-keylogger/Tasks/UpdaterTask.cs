using System;
using System.Threading;

namespace on_screen_keylogger.Tasks
{
    /// <summary>
    /// This class runs a separate daemon <see cref="Thread"/>
    /// that calls <see cref="OnUpdate()"/> every <see cref="UpdateTime"/>
    /// milliseconds.
    /// </summary>
    public abstract class UpdaterTask : IDisposable
    {
        //========================================================
        protected byte _updateTime = 50;
        protected int? _updateFrequency = null;
        //
        private readonly Thread _thread;
        //--------------------------------------------------------
        /// <summary>
        /// The amount of time to wait in milliseconds before
        /// updating the displayed keys. Lower value may cause more lag.
        /// </summary>
        public byte UpdateTime
        {
            get => (byte)Math.Max((int)_updateTime, 10);
            set
            {
                _updateTime = value;
                _updateFrequency = 1000 / _updateTime;
                OnUpdate();
            }
        }

        /// <summary>
        /// How frequently is <see cref="OnUpdate()"/> gonna be called?
        /// </summary>
        public int UpdateFrequency => _updateFrequency ??= (1000 / UpdateTime);
        //========================================================
        public UpdaterTask()
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
        ~UpdaterTask() => Dispose();
        //--------------------------------------------------------
        /// <summary>
        /// This method is called every
        /// <see cref="UpdateTime"/> milliseconds.
        /// </summary>
        public abstract void OnUpdate();
        //--------------------------------------------------------
        /// <summary>
        /// Aborts the thread run by this <see cref="UpdaterTask"/>.
        /// </summary>
        public void Dispose() => _thread.Abort();
        //========================================================
    }
}
