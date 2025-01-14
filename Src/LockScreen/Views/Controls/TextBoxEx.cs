using System.Windows;
using System.Windows.Controls;

namespace LockScreen.Views.Controls
{
    public class TextBoxEx : TextBox
    {
        #region Public Constructors

        public TextBoxEx()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// Detach text box sizing from text size
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint) => Size.Empty;

        #endregion Protected Methods
    }
}
