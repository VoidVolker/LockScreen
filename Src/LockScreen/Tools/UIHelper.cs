using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LockScreen.Tools
{
    /// <summary>
    /// UI helpers
    /// </summary>
    public static class UIHelper
    {
        #region Public Fields

        public const bool False = false;
        public const string I18nPrefix = "I18n";

        public const bool True = true;

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        /// Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child.</param>
        /// <returns>
        /// The first parent item that matches the submitted type parameter. If not matching item
        /// can be found, a null parent is being returned.
        /// </returns>
        public static T FindChild<T>(this DependencyObject parent, string childName = null)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                if (child is not T)
                {
                    // recursively drill down the tree
                    foundChild = child.FindChild<T>(childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }

                    // recursively drill down the tree
                    foundChild = child.FindChild<T>(childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                    {
                        break;
                    }
                    else
                    {
                        // child element found.
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found by it's type without name
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static DependencyObject FindChild(this DependencyObject parent, string childName = null) =>
            FindChild<DependencyObject>(parent, childName);

        /// <summary>
        /// Find resource
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FindRes(this Control control, string name)
        {
            return control.FindResource(name) is string res1
                ? res1
                : FindRes(name);
        }

        /// <summary>
        /// Find global resource
        /// </summary>
        /// <param name="name">resource name</param>
        /// <returns></returns>
        public static string FindRes(string name)
        {
            return Application.Current.FindResource(name) is string res2
                ? res2
                : throw new Exception($"FindRes: Can't find resource {name}");
        }

        /// <summary>
        /// Find global resource of type T
        /// </summary>
        /// <param name="resName">resource name</param>
        /// <returns></returns>
        public static T FindRes<T>(string resName)
        {
            if (Application.Current.FindResource(resName) is T res2)
            {
                return res2;
            }
            //"FindRes: Can't find resource \"" + resName + "\" of type: \"" + typeof(T).Name + "\""
            throw new Exception(
                string.Format(
                    "FindRes: Can't find resource \"{0}\", type \"{1}\"", resName, typeof(T).Name
                )
            );
        }

        // https://stackoverflow.com/a/4031253/2771556
        public static List<T> GetVisualChildCollection<T>(this DependencyObject parent) where T : Visual
        {
            List<T> visualCollection = [];
            GetVisualChildCollection(parent, visualCollection);
            return visualCollection;
        }

        /// <summary>
        /// Find localized string resource with 'I18n' prefix
        /// </summary>
        /// <param name="stringId">string ID</param>
        /// <returns></returns>
        public static string I18n(string stringId) => FindRes(I18nResourceKey(stringId));

        /// <summary>
        /// Find localized resource of type T with 'I18n' prefix
        /// </summary>
        /// <param name="stringId">resource ID</param>
        /// <returns></returns>
        public static T I18n<T>(string stringId) => FindRes<T>(I18nResourceKey(stringId));

        public static string I18nResourceKey(string stringId) => $"{I18nPrefix} {stringId}";

        public static Size MeasureString(this Control control, string text)
            => MeasureString(
                control,
                text,
                control.FontFamily,
                control.FontStyle,
                control.FontStretch,
                control.FontSize,
                control.Foreground,
                control.FontWeight);

        public static Size MeasureString(this Control control, string text, FontWeight weight)
            => MeasureString(
                control,
                text,
                control.FontFamily,
                control.FontStyle,
                control.FontStretch,
                control.FontSize,
                control.Foreground,
                weight);

        public static Size MeasureString(this TextBlock control, string text, FontWeight weight)
            => MeasureString(
                control,
                text,
                control.FontFamily,
                control.FontStyle,
                control.FontStretch,
                control.FontSize,
                control.Foreground,
                weight);

        public static Size MeasureString(
            FrameworkElement control,
            string text,
            FontFamily family,
            FontStyle style,
            FontStretch stretch,
            double fontSize,
            Brush brush,
            FontWeight weight)
        {
            FormattedText formattedText = new(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(family, style, weight, stretch),
                fontSize,
                brush,
                new NumberSubstitution(),
                VisualTreeHelper.GetDpi(control).PixelsPerDip);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                else if (child != null)
                {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }

        #endregion Public Methods
    }
}
