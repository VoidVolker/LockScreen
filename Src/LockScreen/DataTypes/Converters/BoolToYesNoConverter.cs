// Edited: 12.12.2024 1:20:41

using System;
using System.Windows.Data;
 
namespace LockScreen.DataTypes.Converters
{
    /// <summary>
    /// Boolean converter to yes/no localized string
    /// </summary>
    public class BoolToYesNoConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool val && val ? I18n("True") : I18n("False");
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is string val && val == I18n("True");
        }
    }
}
