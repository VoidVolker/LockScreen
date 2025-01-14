using System.Windows;
using System.Windows.Controls;

namespace LockScreen.Views
{
    /// <summary>
    /// Application main view
    /// </summary>
    public class App : Control
    {
        #region Public Constructors

        static App()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(App), new FrameworkPropertyMetadata(typeof(App)));
        }

        /// <summary>
        /// Application main frame
        /// </summary>
        public App()
        {
        }

        #endregion Public Constructors
    }
}
