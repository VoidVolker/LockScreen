using System.Windows;
using System.Windows.Controls;

using LockScreen.Styles;

namespace LockScreen.Views.Controls
{
    /// <summary>
    /// Extended button: <br />
    /// - All buttons have same size
    /// </summary>
    public class ButtonEx : Button
    {
        #region Public Constructors

        public ButtonEx()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override Size MeasureOverride(Size constraint) => AppStyle.ButtonSize(this);

        #endregion Protected Methods
    }
}
