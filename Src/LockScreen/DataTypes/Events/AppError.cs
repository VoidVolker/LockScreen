using System;

namespace LockScreen.DataTypes.Events
{
    /// <summary>
    /// AppError EventArgs
    /// </summary>
    public class AppErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Exceptio details
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// Exception description
        /// </summary>
        public string Description = string.Empty;
    }
}
