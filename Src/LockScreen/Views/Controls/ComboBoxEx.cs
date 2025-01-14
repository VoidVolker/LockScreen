using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using LockScreen.Tools;

namespace LockScreen.Views.Controls
{
    /// <summary>
    /// Combo Box Ex
    /// </summary>
    public class ComboBoxEx : ComboBox
    {
        #region Public Constructors

        /// <summary>
        /// Combo Box Ex
        /// </summary>
        public ComboBoxEx()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        //private FrameworkElement popupContent;

        /// <summary>
        /// OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //if (Template.FindName("PART_Popup", this) is Popup popup
            //    && popup.Child is FrameworkElement content)
            //{
            //popupContent = content;
            Loaded += ResizeToContent;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsSourceProperty, typeof(ComboBox));
            dpd?.AddValueChanged(this, ResizeToContent);
            //}
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Extends combobox to maximum content item width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeToContent(object sender = null, EventArgs e = null)
        {
            Thickness t = BorderThickness;
            Thickness p = Padding;
            double wPadding = p.Left + p.Right + t.Left + t.Right + SystemParameters.VerticalScrollBarWidth;
            //double hPadding = p.Top + p.Bottom + t.Top + t.Bottom;

            foreach (object item in ItemsSource)
            {
                if (item is Visual visual)
                {
                    Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
                    MinWidth = Math.Max(MinWidth, bounds.Width + wPadding);
                }
                else if (item is not null && item.ToString() is string str)
                {
                    Size size = this.MeasureString(str);
                    MinWidth = Math.Max(MinWidth, size.Width + wPadding);
                }
            }
            //popupContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //// suggested in comments, original answer has a static value 19.0
            //double emptySize = /*SystemParameters.VerticalScrollBarWidth +*/ Padding.Left + Padding.Right;
            //MinWidth = emptySize + popupContent.DesiredSize.Width;
            //MinWidth = emptySize + popupContent.ActualWidth;
            UpdateLayout();
            //InvalidateVisual();
        }

        #endregion Private Methods
    }
}
