using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using LockScreen.Tools;

namespace LockScreen.Views.Controls
{
    public class TextBlockEx : TextBlock
    {
        #region Public Constructors

        public TextBlockEx()
        {
            // Fix size changing for different font weights
            var textDp = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(TextBlock));
            var paddingDp = DependencyPropertyDescriptor.FromProperty(PaddingProperty, typeof(TextBlock));
            textDp.AddValueChanged(this, TextChanged);
            paddingDp.AddValueChanged(this, TextChanged);
        }

        #endregion Public Constructors

        #region Private Methods

        // Not working - sealed
        //protected override Size MeasureOverride(Size constraint) =>

        private void TextChanged(object sender, EventArgs e)
        {
            var p = Padding;
            var size = this.MeasureString(Text, FontWeights.Bold);
            MinWidth = size.Width + p.Left + p.Right;
            MinHeight = size.Height + p.Top + p.Bottom;
        }

        #endregion Private Methods
    }
}
