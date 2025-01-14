using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using FontAwesome6;

using LockScreen.DataTypes.Properties;

namespace LockScreen.Views.Controls
{
    public class IconButton : Button
    {
        #region Public Constructors

        public IconButton()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public EFlipOrientation FlipOrientation
        {
            get => (EFlipOrientation)GetValue(FlipOrientationProperty);
            set => SetValue(FlipOrientationProperty, value);
        }

        public Brush HoverBackColor
        {
            get => (Brush)GetValue(HoverBackColorProperty);
            set => SetValue(HoverBackColorProperty, value);
        }

        public Brush HoverPrimaryColor
        {
            get => (Brush)GetValue(HoverPrimaryColorProperty);
            set => SetValue(HoverPrimaryColorProperty, value);
        }

        public Brush HoverSecondaryColor
        {
            get => (Brush)GetValue(HoverSecondaryColorProperty);
            set => SetValue(HoverSecondaryColorProperty, value);
        }

        public EFontAwesomeIcon Icon
        {
            get => (EFontAwesomeIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Brush PrimaryColor
        {
            get => (Brush)GetValue(PrimaryColorProperty);
            set => SetValue(PrimaryColorProperty, value);
        }

        public double? PrimaryOpacity
        {
            get => (double?)GetValue(PrimaryOpacityProperty);
            set => SetValue(PrimaryOpacityProperty, value);
        }

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        public Brush SecondaryColor
        {
            get => (Brush)GetValue(SecondaryColorProperty);
            set => SetValue(SecondaryColorProperty, value);
        }

        public double? SecondaryOpacity
        {
            get => (double?)GetValue(SecondaryOpacityProperty);
            set => SetValue(SecondaryOpacityProperty, value);
        }

        public Brush SelectedBackColor
        {
            get => (Brush)GetValue(SelectedBackColorProperty);
            set => SetValue(SelectedBackColorProperty, value);
        }

        public Brush SelectedPrimaryColor
        {
            get => (Brush)GetValue(SelectedPrimaryColorProperty);
            set => SetValue(SelectedPrimaryColorProperty, value);
        }

        public Brush SelectedSecondaryColor
        {
            get => (Brush)GetValue(SelectedSecondaryColorProperty);
            set => SetValue(SelectedSecondaryColorProperty, value);
        }

        public bool? SwapOpacity
        {
            get => (bool?)GetValue(SwapOpacityProperty);
            set => SetValue(SwapOpacityProperty, value);
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty FlipOrientationProperty =
            DP<IconButton>.R(x => x.FlipOrientation);

        public static readonly DependencyProperty HoverBackColorProperty =
            DP<IconButton>.R(x => x.HoverBackColor, ButtonColors.SelectedBackgroundColor);

        public static readonly DependencyProperty HoverPrimaryColorProperty =
            DP<IconButton>.R(x => x.HoverPrimaryColor, SystemColors.WindowTextBrush);

        public static readonly DependencyProperty HoverSecondaryColorProperty =
            DP<IconButton>.R(x => x.HoverSecondaryColor, SystemColors.GrayTextBrush);

        public static readonly DependencyProperty IconProperty =
            DP<IconButton>.R(x => x.Icon);

        public static readonly DependencyProperty IconSizeProperty =
            DP<IconButton>.R(x => x.IconSize, DefaultIconSize);

        public static readonly DependencyProperty PrimaryColorProperty =
            DP<IconButton>.R(x => x.PrimaryColor, SystemColors.WindowTextBrush);

        public static readonly DependencyProperty PrimaryOpacityProperty =
            DP<IconButton>.R(x => x.PrimaryOpacity);

        public static readonly DependencyProperty RotationProperty =
            DP<IconButton>.R(x => x.Rotation);

        public static readonly DependencyProperty SecondaryColorProperty =
            DP<IconButton>.R(x => x.SecondaryColor, SystemColors.GrayTextBrush);

        public static readonly DependencyProperty SecondaryOpacityProperty =
            DP<IconButton>.R(x => x.SecondaryOpacity);

        public static readonly DependencyProperty SelectedBackColorProperty =
            DP<IconButton>.R(x => x.SelectedBackColor, ButtonColors.ControlMouseOverColor);

        public static readonly DependencyProperty SelectedPrimaryColorProperty =
            DP<IconButton>.R(x => x.SelectedPrimaryColor, SystemColors.WindowTextBrush);

        public static readonly DependencyProperty SelectedSecondaryColorProperty =
            DP<IconButton>.R(x => x.SelectedSecondaryColor, SystemColors.GrayTextBrush);

        public static readonly DependencyProperty SwapOpacityProperty =
            DP<IconButton>.R(x => x.SwapOpacity);

        #endregion Public Fields

        #region Private Fields

        private const double DefaultIconSize = 12;

        #endregion Private Fields

        #region Public Classes

        // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/button-styles-and-templates?view=netframeworkdesktop-4.8&viewFallbackFrom=netdesktop-8.0
        public static class ButtonColors
        {
            #region Public Fields

            public static readonly Brush ContentAreaColorDark = From(0xFF7381F9);
            public static readonly Brush ContentAreaColorLight = From(0xFFC5CBF9);
            public static readonly Brush ControlDarkColor = From(0xFF211AA9);
            public static readonly Brush ControlLightColor = From(Colors.White);
            public static readonly Brush ControlMediumColor = From(0xFF7381F9);
            public static readonly Brush ControlMouseOverColor = From(0x773843C4);
            public static readonly Brush ControlPressedColor = From(0xFF211AA9);
            public static readonly Brush DisabledControlDarkColor = From(0xFFC5CBF9);
            public static readonly Brush DisabledControlLightColor = From(0xFFE8EDF9);
            public static readonly Brush DisabledForegroundColor = From(0xFF888888);
            public static readonly Brush SelectedBackgroundColor = From(0xFFC5CBF9);
            public static readonly Brush SelectedUnSelectedColor = From(0xFFDDDDDD);
            public static readonly Brush WindowColor = From(0xFFE8EDF9);

            #endregion Public Fields

            #region Public Methods

            public static Brush From(uint n)
            {
                var c = System.Drawing.Color.FromArgb((int)n);
                return new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
            }

            public static Brush From(Color c) => new SolidColorBrush(c);

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}
