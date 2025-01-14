using System.Windows;

using LockScreen.Views.Controls;

namespace LockScreen.Views.Tabs
{
    public class About : TabBase
    {
        #region Public Constructors

        static About()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(About), new FrameworkPropertyMetadata(typeof(About)));
        }

        public About()
        {
        }

        #endregion Public Constructors
    }
}
