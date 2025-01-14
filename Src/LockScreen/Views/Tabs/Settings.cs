using System.Windows;

using LockScreen.Views.Controls;

namespace LockScreen.Views.Tabs
{
    public class Settings : TabBase
    {
        #region Public Constructors

        static Settings()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Settings), new FrameworkPropertyMetadata(typeof(Settings)));
        }

        public Settings()
        {
        }

        #endregion Public Constructors
    }
}
