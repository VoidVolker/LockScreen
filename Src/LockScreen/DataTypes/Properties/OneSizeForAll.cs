//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;

//namespace LockScreen.DataTypes.Properties
//{
//    /// <summary>
//    /// Runtime size synchronization mechanism for controls.
//    /// </summary>
//    /// <remarks>
//    /// Interesting idea, but doesn't work in WPF as expected:
//    /// WPF controls doesn't support IDisposable interface
//    /// and I wasn't able to find simple solution to catch
//    /// control disposing event.
//    /// </remarks>
//    public class OneSizeForAll
//    {
//        #region Public Fields

//        public const string ContentKey = "Content";

//        private bool IsResizing = false;

//        #endregion Public Fields

//        #region Public Methods

//        public OneSizeForAll()
//        {
//        }

//        public double Height = 0;

//        public HashSet<object> Items = [];

//        public double Width = 0;

//        public void AttachContent(ContentControl control)
//        {
//            DependencyPropertyDescriptor dp = DependencyPropertyDescriptor.FromProperty(
//                ContentControl.ContentProperty, control.GetType());

//            dp.RemoveValueChanged(control, ResizeOnContentChanged);
//            dp.AddValueChanged(control, ResizeOnContentChanged);

//            Items.Add(control);
//        }

//        public void AttachContentSizeChanged(FrameworkElement control)
//        {
//            FrameworkElement content = control.FindChild<FrameworkElement>(ContentKey);

//            if (content is not null)
//            {
//                content.SizeChanged -= ResizeOnContentChanged;
//                content.SizeChanged += ResizeOnContentChanged;
//                Items.Add(control);
//                //ResizeOnContentChanged(control);
//            }
//        }

//        public void Detach(FrameworkElement control)
//        {
//            FrameworkElement content = control.FindChild<FrameworkElement>(ContentKey);
//            if (content is not null)
//            {
//                content.SizeChanged -= ResizeOnContentChanged;
//            }

//            Items.Remove(control);
//        }

//        public void Detach(ContentControl control)
//        {
//            DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, control.GetType())
//                .RemoveValueChanged(control, ResizeOnContentChanged);

//            Items.Remove(control);
//        }

//        private void ResizeOnContentChanged(object sender, EventArgs e = null)
//        {
//            if (IsResizing) { return; }
//            IsResizing = true;

//            double w, h;
//            Width = Height = 0;

//            foreach (object item in Items)
//            {
//                Thickness t = new(0);
//                Thickness p = new(0);
//                object content = null;

//                if (item is ContentControl cntControl)
//                {
//                    t = cntControl.BorderThickness;
//                    p = cntControl.Padding;
//                    content = cntControl.Content;
//                }
//                else if (item is FrameworkElement feControl)
//                {
//                    content = feControl.FindChild<FrameworkElement>(ContentKey);
//                }
//                else
//                {
//                    continue;   // Unknown item type
//                }

//                double wPadding = p.Left + p.Right + t.Left + t.Right;
//                double hPadding = p.Top + p.Bottom + t.Top + t.Bottom;

//                if (content is string str && item is Control control)
//                {
//                    Size size = control.MeasureString(str);
//                    w = size.Width;
//                    h = size.Height;
//                }
//                else if (content is FrameworkElement el)
//                {
//                    Rect bounds = VisualTreeHelper.GetDescendantBounds(el);
//                    w = bounds.Width;
//                    h = bounds.Height;
//                }
//                else
//                {
//                    continue;   // Unknown content type
//                }

//                Width = Math.Max(Width, w + wPadding);
//                Height = Math.Max(Height, h + hPadding);
//            }

//            foreach (FrameworkElement item in Items.Cast<FrameworkElement>())
//            {
//                item.MinWidth = Width;
//                item.MinHeight = Height;
//            }

//            IsResizing = false;
//        }

//        #endregion Public Methods
//    }
//}
