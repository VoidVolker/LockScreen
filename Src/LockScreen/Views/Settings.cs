using System.Windows;
using System.Windows.Controls;

namespace LockScreen.Views
{
    /// <summary>
    /// Settings control
    /// </summary>
    public class Settings : Control
    {
        #region Public Constructors

        static Settings()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Settings), new FrameworkPropertyMetadata(typeof(Settings)));
        }

        /// <summary>
        /// Application main frame
        /// </summary>
        public Settings()
        {
        }

        #endregion Public Constructors
    }
}
