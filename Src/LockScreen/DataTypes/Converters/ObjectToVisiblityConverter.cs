using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;

namespace LockScreen.DataTypes.Converters
{
    /// <summary>
    /// Object to visibility converter
    /// </summary>
    public class ObjectToVisiblityConverter : IValueConverter
    {
        /// <summary>
        /// Convert object visibility: visible, collapsed or hidden
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Not <br /> Hidden <br /> Not | Hidden</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0066:Преобразовать оператор switch в выражение", Justification = "<Ожидание>")]
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v1 = Visibility.Visible
                , v2 = Visibility.Collapsed
            ;
            bool isHiddenMode = false;
            bool isNot = false;
            if (parameter is string p)
            {
                string[] options = p.Split(" | ");
                foreach (string opt in options)
                {
                    switch (opt)
                    {
                        case "Not":
                            isNot = true;
                            break;

                        case "Hidden":
                            isHiddenMode = true;
                            break;
                    }
                }

                if (isNot && isHiddenMode == false)
                {
                    v1 = Visibility.Collapsed;
                    v2 = Visibility.Visible;
                }
                else if (isNot && isHiddenMode)
                {
                    v1 = Visibility.Hidden;
                    v2 = Visibility.Visible;
                }
                else if (isNot == false && isHiddenMode)
                {
                    v2 = Visibility.Hidden;
                }
            }

            switch (value)
            {
                case null:
                    return v2;

                case bool boolean when boolean == false:
                    return v2;

                case byte b when b == 0:
                    return v2;

                case short n when n == 0:
                    return v2;

                case int i when i == 0:
                    return v2;

                case long l when l == 0:
                    return v2;

                case float f when f == 0.0:
                    return v2;

                case double d when d == 0.0:
                    return v2;

                case string s when string.IsNullOrWhiteSpace(s):
                    return v2;

                default:
                    return v1;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Visible;
        }
    }
}
