using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using LockScreen.DataTypes.Events;
using LockScreen.DataTypes.Properties;
using LockScreen.Tools;

namespace LockScreen.Views.Controls
{
    public class Switch : DependencyObject /*FrameworkElement*/
    {
        #region Public Properties

        /// <summary>
        /// Switch content - string or any Control/FrameworkElement/Visual etc.
        /// </summary>
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Dynamic resource I18n short ID (whthout I18n prefix)
        /// </summary>
        public string I18n { get; set; }

        /// <summary>
        /// Value to compare
        /// </summary>
        public object When
        {
            get => GetValue(WhenProperty);
            set => SetValue(WhenProperty, value);
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty ContentProperty = DP<Switch>.R(
            x => x.Content,
            x => x.OnContentChanged);

        public static readonly DependencyProperty WhenProperty = DP<Switch>.R(x => x.When);

        #endregion Public Fields

        #region Private Methods

        private void OnContentChanged(DependencyPropertyChangedEventArgs<object> e)
        {
            ContentChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion Private Methods

        #region Public Events

        public event EventHandler ContentChanged;

        #endregion Public Events
    }

    public class SwitchButton : Button
    {
        #region Public Constructors

        public SwitchButton()
        {
            Switches = [];
            // Add collection change trigger
            LockScreen.App.LanguageChanged += App_LanguageChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Switch> Switches
        {
            get { return (ObservableCollection<Switch>)GetValue(SwitchesProperty); }
            set { SetValue(SwitchesProperty, value); }
        }

        public object When
        {
            get { return GetValue(SwitchProperty); }
            set { SetValue(SwitchProperty, value); }
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty SwitchesProperty =
            DP<SwitchButton>.R(
                x => x.Switches,
                x => x.OnSwitchesChanged);

        public static readonly DependencyProperty SwitchProperty =
            DP<SwitchButton>.R(
                x => x.When,
                x => x.OnSwitchChanged);

        #endregion Public Fields

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SwhitchesRefresh();
            Switch();
            InvalidateVisual();
        }

        /// <summary>
        /// Apply switch for value
        /// </summary>
        /// <param name="value"></param>
        public void Switch(object value)
        {
            foreach (Switch sw in Switches)
            {
                if (sw.When.Equals(value))
                {
                    Content = sw.Content;
                    return;
                }
            }
        }

        /// <summary>
        /// Apply current value switch
        /// </summary>
        public void Switch() => Switch(When);

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            SwhitchesRefresh();
            Switch();
            //InvalidateVisual();
        }

        #endregion Public Methods

        #region Private Methods

        private void El_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SwhitchesRefresh();
            Switch();
        }

        private void OnSwitchChanged(DependencyPropertyChangedEventArgs<object> e)
        {
            Switch();
            //InvalidateVisual();
        }

        private void OnSwitchesChanged(DependencyPropertyChangedEventArgs<ObservableCollection<Switch>> e)
        {
            SwhitchesRefresh();
            Switch();
            //InvalidateVisual();
        }

        private void Sw_ContentChanged(object sender, EventArgs e)
        {
            SwhitchesRefresh();
            if (sender is Switch sw && sw.When.Equals(When))
            {
                Content = sw.Content;
            }
            //InvalidateVisual();
        }

        private void SwhitchesRefresh()
        {
            // Measure spacing out of client size
            Thickness t = BorderThickness;
            Thickness p = Padding;
            double wPadding = p.Left + p.Right + t.Left + t.Right;
            double hPadding = p.Top + p.Bottom + t.Top + t.Bottom;
            double wMin = 0;
            double hMin = 0;
            // Measure content size and set minimal item size to biggest content item to avoid item
            // size jumping
            foreach (Switch sw in Switches)
            {
                sw.ContentChanged -= Sw_ContentChanged; // For case if event handler was already added
                sw.ContentChanged += Sw_ContentChanged;

                if (sw.I18n is not null)
                {
                    sw.Content = I18n(sw.I18n);
                }

                double w, h;

                if (sw.Content is string str)
                {
                    Size size = this.MeasureString(str);
                    w = size.Width;
                    h = size.Height;
                }
                else if (sw.Content is FrameworkElement el)
                {
                    Rect bounds = VisualTreeHelper.GetDescendantBounds(el);
                    w = bounds.Width;
                    h = bounds.Height;
                    el.SizeChanged -= El_SizeChanged; // For case if event handler was already added
                    el.SizeChanged += El_SizeChanged;
                }
                else
                {
                    continue;
                }

                wMin = Math.Max(wMin, w + wPadding);
                hMin = Math.Max(hMin, h + hPadding);
            }

            MinWidth = wMin;
            MinHeight = hMin;
        }

        #endregion Private Methods
    }
}
