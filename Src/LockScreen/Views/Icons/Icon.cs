using System.Windows;
using System.Windows.Controls;

namespace LockScreen.Views.Icons
{
    /// <summary>
    /// Icon
    /// </summary>
    public class Icon : Control
    {
        #region Public Constructors

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        /// <summary>
        /// Icon
        /// </summary>
        public Icon()
        {
        }

        #endregion Public Constructors
    }
}
