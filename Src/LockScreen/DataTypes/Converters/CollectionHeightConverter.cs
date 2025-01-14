using System;
using System.Collections;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LockScreen.DataTypes.Converters
{
    public class CollectionHeightConverter : IMultiValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not IEnumerable collection) { return 0; }
            if (values[1] is not Control control) { return 0; }

            var t = control.BorderThickness;
            var p = control.Padding;
            double hPadding = p.Top + p.Bottom + t.Top + t.Bottom;

            double min = 0;
            foreach (var item in collection)
            {
                double h = 0;

                if (item is Visual visual)
                {
                    h = VisualTreeHelper.GetDescendantBounds(visual).Height;
                }
                else
                {
                    string str = item is string str1
                        ? str1
                        : item.ToString() is string str2
                            ? str2
                            : string.Empty;
                    h = Tools.UIHelper.MeasureString(control, str).Height;
                }

                min = Math.Max(min, h + hPadding);
            }
            return min;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
