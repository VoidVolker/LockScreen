using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LockScreen.Views.Controls
{
    public class ScreensList : DataGrid
    {
        #region Public Constructors

        #region Public Constructors

        static ScreensList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreensList), new FrameworkPropertyMetadata(typeof(ScreensList)));
        }

        public ScreensList()
        {
            //LockScreen.App.LanguageChanged += App_LanguageChanged;
        }

        #endregion Public Constructors

        private void App_LanguageChanged(object sender = null, System.EventArgs e = null)
        {
            //List<DataGridColumnHeader> headers = this.GetVisualChildCollection<DataGridColumnHeader>();
            //Debug.WriteLine($"-- Headers --");
            //foreach (var h in headers)
            //{
            //    Rect b = VisualTreeHelper.GetDescendantBounds(h);
            //    Debug.WriteLine($"Header bounds w,h 1: {b.Width}, {b.Height}");
            //    Debug.WriteLine($"Header actual w,h 2: {h.ActualWidth}, {h.ActualHeight}\n");
            //}
            //List<DataGridColumn> columns = [];
            //foreach (var c in Columns)
            //{
            //    //columns.Add(c);
            //    //var ct = c.HeaderTemplate;
            //    //c.HeaderTemplate = null;
            //    //c.HeaderTemplate = ct;
            //    //c.MaxWidth = 10;
            //    //c.MaxWidth = double.PositiveInfinity;

            //    //c.Visibility = Visibility.Collapsed;
            //    //c.Visibility = Visibility.Visible;
            //    var h = c.Header;
            //    c.Header = null;
            //}

            //Columns.Clear();
            //Columns.AddRange(columns);

            //var t = Template;
            //Template = null;
            //Template = t;

            //HeadersVisibility = DataGridHeadersVisibility.None;
            //HeadersVisibility = DataGridHeadersVisibility.Column;

            //ApplyTemplate();

            ////double w = 0;
            //DataGridColumn starColumn = null;
            //foreach (DataGridColumn col in Columns)
            //{
            //    //w += col.ActualWidth;
            //    //Debug.Write($"{col.ActualWidth:F2} ");
            //    if (col.Width.IsStar)
            //    {
            //        col.Width = 0;
            //        starColumn = col;
            //        break;
            //    }
            //}
            //InvalidateVisual();
            //starColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            //InvalidateVisual();
            ////Debug.Write($"- {w:F2}\n");
        }

        #endregion Public Constructors

        //#region Public Methods

        #region Public Methods

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    // Fix for initial wrong column size with "*" width (smaller then available space)
        //    LayoutUpdated += Resize;
        //    //SizeChanged += Resize;
        //    //App_LanguageChanged();
        //    //Resize();
        //}

        //#endregion Public Methods

        //#region Private Methods

        //private void Resize(object sender = null, EventArgs e = null)
        //{
        //    if (ItemsSource == null || Columns.Count == 0) { return; }

        //    Debug.WriteLine($"ActualWidth: {ActualWidth}");

        //    double colWidth = Columns[0].ActualWidth;
        //    if (Columns.All(c => c.ActualWidth == colWidth))
        //    {
        //        return;
        //    }

        //    double width = ActualWidth;
        //    DataGridColumn starColumn = null;
        //    foreach (DataGridColumn col in Columns)
        //    {
        //        Debug.WriteLine($"col.ActualWidth: {col.ActualWidth} / star:{col.Width.IsStar}");
        //        if (col.Width.IsStar)
        //        {
        //            starColumn = col;
        //            continue;
        //        }
        //        width -= col.ActualWidth;
        //    }

        //    if (starColumn is not null)
        //    {
        //        Debug.WriteLine($"Last column ActualWidth: {starColumn.ActualWidth}");
        //        Debug.WriteLine($"width: {width}");
        //        //LayoutUpdated -= Resize;
        //        //var w1 = starColumn.Width;
        //        //starColumn.MinWidth = width;
        //        starColumn.MaxWidth = width;
        //        //starColumn.Width = w1;
        //        //Debug.WriteLine($"Last column width: {width}");
        //    }
        //}

        #endregion Public Methods
    }
}
