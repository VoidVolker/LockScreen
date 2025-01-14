using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LockScreen.DataTypes.Properties;
using LockScreen.DataTypes.Structures;
using LockScreen.Exceptions;

namespace LockScreen.Views.Controls
{
    public class Tabs : ItemsControl
    {
        #region Public Constructors

        static Tabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tabs), new FrameworkPropertyMetadata(typeof(Tabs)));
        }

        public Tabs()
        {
            Buttons = [];
            Collection = [];
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Tabs buttons
        /// </summary>
        public ObservableCollection<Control> Buttons
        {
            get { return (ObservableCollection<Control>)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        /// <summary>
        /// Can content scroll
        /// </summary>
        public bool CanContentScroll
        {
            get { return (bool)GetValue(CanContentScrollProperty); }
            set { SetValue(CanContentScrollProperty, value); }
        }

        /// <summary>
        /// Tabs collection
        /// </summary>
        public ObservableCollection<Tab> Collection
        {
            get { return (ObservableCollection<Tab>)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        /// <summary>
        /// Free space near tabs
        /// </summary>
        public FrameworkElement Header
        {
            get { return (Control)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Selected tab control
        /// </summary>
        public Tab Selected
        {
            get => (Tab)GetValue(SelectedProperty);
            set
            {
                SetValue(SelectedProperty, value);
                SetValue(SelectedContentProperty, value.Content);
            }
        }

        public Control SelectedContent
        {
            get { return (Control)GetValue(SelectedContentProperty); }
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty ButtonsProperty =
            DP<Tabs>.R(x => x.Buttons);

        public static readonly DependencyProperty CanContentScrollProperty =
            DP<Tabs>.R(x => x.CanContentScroll, false);

        public static readonly DependencyProperty CollectionProperty =
            DP<Tabs>.R(x => x.Collection);

        public static readonly DependencyProperty HeaderProperty =
            DP<Tabs>.R(x => x.Header);

        public static readonly DependencyProperty SelectedContentProperty =
            DP<Tabs>.R(x => x.SelectedContent);

        public static readonly DependencyProperty SelectedProperty =
            DP<Tabs>.R(x => x.Selected, defaultValue: null);

        #endregion Public Fields

        ///// <summary>
        ///// Tabs orientation
        ///// </summary>
        //public Orientation Orientation
        //{
        //    get { return (Orientation)GetValue(OrientationProperty); }
        //    set { SetValue(OrientationProperty, value); }
        //}

        //public static readonly DependencyProperty OrientationProperty =
        //  DependencyProperty<Tabs>.Register(x => x.Orientation, Orientation.Horizontal);

        #region Private Fields

        /// <summary>
        /// Buttons container
        /// </summary>
        private StackPanel ButtonsContainer;

        private ItemsChangedDelegate ItemsChangedCallback = () => { };

        /// <summary>
        /// Scroll container
        /// </summary>
        private ScrollContentPresenter Scroll;

        #endregion Private Fields

        #region Private Delegates

        private delegate void ItemsChangedDelegate();

        #endregion Private Delegates

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Scroll = this.FindChild<ScrollContentPresenter>(nameof(Scroll));
            ButtonsContainer = this.FindChild<StackPanel>(nameof(Buttons));
            //ContentPresenter HeaderContent = this.FindChild<ContentPresenter>("HeaderContent");

            if (Scroll is null) { throw new ElementNotFoundException(typeof(ScrollContentPresenter), "Scroll"); }
            if (ButtonsContainer is null) { throw new ElementNotFoundException(typeof(StackPanel), "Buttons"); }
            //if (HeaderContent is null) { throw new ElementNotFoundException(typeof(ContentPresenter), "HeaderContent"); }

            //Header = new UIElementCollection(HeaderContent, HeaderContent);
            ItemsChangedCallback = UpdateTabs;
            UpdateTabs();
        }

        public void Select(Tab tab)
        {
            if (Selected != null)
            {
                Selected.IsActive = false;
                Selected.Content.Visibility = Visibility.Hidden;
                if (Selected.Button is TabButton btn1)
                {
                    btn1.IsActive = false;
                }
            }

            Selected = tab;
            Selected.IsActive = true;
            Selected.Content.Visibility = Visibility.Visible;
            if (Selected.Button is TabButton btn2)
            {
                btn2.IsActive = true;
            }
        }

        public void Select(ushort index)
        {
            if (index >= Collection.Count) { throw new ArgumentOutOfRangeException(nameof(index)); };
            Select(Collection[index]);
        }

        public void SelectButton(Control tabButton)
        {
            if (TabTryGetFromButton(tabButton, out Tab tab))
            {
                Select(tab);
            }
        }

        public void SelectContent(Control tabContent)
        {
            if (TabTryGetFromContent(tabContent, out Tab tab))
            {
                Select(tab);
            }
        }

        public bool TabTryGetFromButton(Control tabButton, out Tab tab)
        {
            foreach (Tab t in Collection)
            {
                if (t.Button == tabButton)
                {
                    tab = t;
                    return true;
                }
            }
            tab = null;
            return false;
        }

        public bool TabTryGetFromContent(Control tabContent, out Tab tab)
        {
            foreach (Tab t in Collection)
            {
                if (t.Content == tabContent)
                {
                    tab = t;
                    return true;
                }
            }
            tab = null;
            return false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            ItemsChangedCallback();
        }

        #endregion Protected Methods

        #region Private Methods

        private void TabButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SelectButton(sender as Control);
            e.Handled = true;
        }

        private void UpdateTabs()
        {
            if (Items.Count == 0)
            {
                Selected = null;
                Collection.Clear();
                return;
            }

            ButtonsContainer.Children.Clear();
            ushort i = 0;
            foreach (Control item in Items)
            {
                Tab tab = new()
                {
                    Content = item,
                    Index = i
                };

                if (Buttons != null && Buttons.Count > i)
                {
                    tab.Button = Buttons[i];
                    tab.Button.PreviewMouseDown += TabButton_MouseUp;
                    ButtonsContainer.Children.Add(tab.Button);
                    if (tab.Button is TabButton btn)
                    {
                        btn.Index = i;
                    }
                }

                if (Selected is not null && Selected.IsActive)
                {
                    tab.IsActive = true;
                    Select(tab);
                }

                Collection.Add(tab);
                i++;
            }

            // Check if selected item in list
            if (!(Selected is not null && Items.Contains(Selected.Content)))
            {
                // Select default item
                Select(Collection[0]);
            }
        }

        #endregion Private Methods
    }
}
