using System.Windows;
using System.Windows.Controls;

using LockScreen.DataTypes.Events;
using LockScreen.DataTypes.Properties;
using LockScreen.Styles;

namespace LockScreen.Views.Controls
{
    public class TabButton : Button
    {
        #region Public Fields

        public static readonly FontWeight ActiveFontWeight = FontWeights.DemiBold;

        #endregion Public Fields

        #region Public Constructors

        static TabButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabButton), new FrameworkPropertyMetadata(typeof(TabButton)));
        }

        public TabButton()
        {
            BottomPlaceholderBorder = new Thickness(0);
            IsHitTestVisible = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override Size MeasureOverride(Size constraint) => AppStyle.TabButtonSize(this);

        #endregion Protected Methods

        #region Public Properties

        public Thickness BottomPlaceholderBorder
        {
            get { return (Thickness)GetValue(BottomPlaceholderBorderProperty); }
            set { SetValue(BottomPlaceholderBorderProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ushort Index
        {
            get { return (ushort)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        #endregion Public Properties



        #region Public Fields

        public static readonly DependencyProperty BottomPlaceholderBorderProperty =
            DP<TabButton>.R(x => x.BottomPlaceholderBorder);

        public static readonly DependencyProperty HeaderProperty =
                    DP<TabButton>.R(x => x.Header, string.Empty);

        public static readonly DependencyProperty IndexProperty =
            DP<TabButton>.R(x => x.Index, (ushort)0);

        public static readonly DependencyProperty IsActiveProperty =
            DP<TabButton>.R(
                x => x.IsActive,
                false,
                x => x.OnIsActiveChnaged);

        #endregion Public Fields

        //private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TabButton btn = (TabButton)d;
        //    string text = (string)e.NewValue;
        //    btn.MinWidth = btn.MeasureString(text, FontWeights.DemiBold).Width + 22;
        //}

        //[SuppressMessage("Style", "IDE0045:Преобразовать в условное выражение", Justification = "<Ожидание>")]
        //private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TabButton btn = (TabButton)d;
        //    Thickness t = btn.BorderThickness;
        //    ushort index = (ushort)e.NewValue;

        //    if (index == 0)
        //    {
        //        btn.BorderThickness = new Thickness(1, t.Top, t.Right, t.Bottom);
        //    }
        //    else
        //    {
        //        btn.BorderThickness = new Thickness(0, t.Top, t.Right, t.Bottom);
        //    }
        //}

        #region Private Fields

        private Thickness bottomThinkess = new(0, 0, 0, 1);
        private Thickness emptyThinkess = new(0);

        #endregion Private Fields

        #region Private Methods

        private void OnIsActiveChnaged(DependencyPropertyChangedEventArgs<bool> e)
        {
            Thickness t = BorderThickness;
            bool isActive = e.NewValue;

            if (isActive)
            {
                BorderThickness = new Thickness(t.Left, t.Top, t.Right, 0);
                BottomPlaceholderBorder = bottomThinkess;
                FontWeight = ActiveFontWeight;
            }
            else
            {
                BorderThickness = new Thickness(t.Left, t.Top, t.Right, 1);
                BottomPlaceholderBorder = emptyThinkess;
                FontWeight = FontWeights.Normal;
            }
        }

        #endregion Private Methods
    }
}
