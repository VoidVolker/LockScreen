using System;
using System.Windows;
using System.Windows.Data;

namespace LockScreen.DataTypes.Converters
{
    /// <summary>
    /// Defines the <see cref="BoolVisiblityConverter" />
    /// </summary>
    public class BoolVisiblityConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool flag && flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Visible;
        }
    }
}