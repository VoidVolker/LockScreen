using System.Windows;

using LockScreen.Views.Controls;

namespace LockScreen.Views.Tabs
{
    public class LockScreen : TabBase
    {
        #region Public Constructors

        static LockScreen()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LockScreen), new FrameworkPropertyMetadata(typeof(LockScreen)));
        }

        public LockScreen()
        {
        }

        #endregion Public Constructors
    }
}
