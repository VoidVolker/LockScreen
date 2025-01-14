using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using LockScreen.DataTypes.Collections.I18n;
using LockScreen.Tools;
using LockScreen.Views.Controls;

namespace LockScreen.Styles
{
    /// <summary>
    /// Application style management
    /// </summary>
    public class AppStyle
    {
        #region Private Fields

        /// <summary>
        /// Controls sizes cache
        /// </summary>
        private static readonly Dictionary<ControlCID, Size> SizesCache = [];

        /// <summary>
        /// Tab I18n key prefix
        /// </summary>
        private static readonly string TabKeyPrefix = I18nResourceKey("Tab");

        /// <summary>
        /// Resource kind values cache
        /// </summary>
        private static readonly Dictionary<ResourceCID, List<string>> ValuesCache = [];

        #endregion Private Fields

        #region Public Methods

        static AppStyle()
        {
            App.LanguageChanged += ClearCaches;
        }

        /// <summary>
        /// Return button size
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static Size ButtonSize(Control btn) => ResourceSize(btn, suffixes: ["button"], keys: [I18nServiceControl.ResourceKey]);

        /// <summary>
        /// Return size for control and resource kind
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="prefixes"></param>
        /// <param name="suffixes"></param>
        /// <param name="keys"></param>
        /// <param name="fontWeight"></param>
        /// <returns></returns>
        public static Size ResourceSize(Control cnt, string[] prefixes = null, string[] suffixes = null, string[] keys = null, FontWeight? fontWeight = null)
        {
            ResourceCID resKindCid = new(prefixes, suffixes, keys);
            ControlCID controlCid = new(cnt, resKindCid);

            if (SizesCache.TryGetValue(controlCid, out Size size))
            {
                //Debug.WriteLine("ResourceSize - size from cache");
                return size;
            }

            List<string> stringValues;
            if (ValuesCache.TryGetValue(resKindCid, out List<string> cachedValues))
            {
                //Debug.WriteLine("ResourceSize - values from cache");
                stringValues = cachedValues;
            }
            else
            {
                //Debug.WriteLine("ResourceSize - values cached");
                stringValues = ResourceCID.Load(prefixes, suffixes, keys);
                ValuesCache.Add(resKindCid, stringValues);
            }

            Size resultSize = controlCid.MeasureStringsMax(cnt, stringValues, fontWeight);
            SizesCache.Add(controlCid, resultSize);
            //Debug.WriteLine("ResourceSize - size calculated and cached");

            return resultSize;
        }

        /// <summary>
        /// Return tab button size
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static Size TabButtonSize(Control tab) => ResourceSize(tab, prefixes: [TabKeyPrefix], fontWeight: TabButton.ActiveFontWeight);

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Clear all caches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ClearCaches(object sender, EventArgs e)
        {
            SizesCache.Clear();
            ValuesCache.Clear();
        }

        #endregion Private Methods

        #region Private Classes

        /// <summary>
        /// Control Cache ID
        /// </summary>
        private class ControlCID : IEquatable<ControlCID>
        {
            #region Public Constructors

            public ControlCID(Control cnt, ResourceCID resKindCid)
            {
                FontFamily = cnt.FontFamily;
                FontSize = cnt.FontSize;
                FontStretch = cnt.FontStretch;
                FontStyle = cnt.FontStyle;
                FontWeight = cnt.FontWeight;
                Foreground = cnt.Foreground;

                Thickness t = cnt.BorderThickness;
                Thickness p = cnt.Padding;
                WPadding = p.Left + p.Right + t.Left + t.Right;
                HPadding = p.Top + p.Bottom + t.Top + t.Bottom;
                ResourceKindCID = resKindCid;

                Hash =
                    Foreground.GetHashCode() ^
                    FontFamily.GetHashCode() ^
                    FontSize.GetHashCode() ^
                    FontStretch.GetHashCode() ^
                    FontStyle.GetHashCode() ^
                    FontWeight.GetHashCode() ^
                    WPadding.GetHashCode() ^
                    HPadding.GetHashCode() ^
                    ResourceKindCID.GetHashCode();
            }

            #endregion Public Constructors

            #region Public Fields

            public readonly FontFamily FontFamily;
            public readonly double FontSize;
            public readonly FontStretch FontStretch;
            public readonly FontStyle FontStyle;
            public readonly FontWeight FontWeight;
            public readonly Brush Foreground;
            public readonly double HPadding = 0;
            public readonly ResourceCID ResourceKindCID;
            public readonly double WPadding = 0;

            #endregion Public Fields

            #region Private Fields

            private readonly int Hash = 0;

            #endregion Private Fields

            #region Public Methods

            public bool Equals(ControlCID other) =>
                other is not null &&
                    ResourceKindCID.Equals(other.ResourceKindCID) &&
                    Foreground.Equals(other.Foreground) &&
                    FontFamily.Equals(other.FontFamily) &&
                    FontSize.Equals(other.FontSize) &&
                    FontStretch.Equals(other.FontStretch) &&
                    FontStyle.Equals(other.FontStyle) &&
                    FontWeight.Equals(other.FontWeight);

            public override bool Equals(object obj) => Equals(obj as ControlCID);

            public override int GetHashCode() => Hash;

            public Size MeasureStringsMax(Control cnt, List<string> strings, FontWeight? weight = null)
            {
                double w = 0;
                double h = 0;

                foreach (string value in strings)
                {
                    Size s = weight is FontWeight fontWeight
                        ? cnt.MeasureString(value, fontWeight)
                        : cnt.MeasureString(value);
                    w = Math.Max(w, s.Width);
                    h = Math.Max(h, s.Height);
                }

                return new(w + WPadding, h + HPadding);
            }

            #endregion Public Methods
        }

        /// <summary>
        /// Resource kind cache ID
        /// </summary>
        private class ResourceCID : IEquatable<ResourceCID>
        {
            #region Private Fields

            private readonly int Hash = 0;

            #endregion Private Fields

            #region Public Constructors

            public ResourceCID(string[] prefixes = null, string[] suffixes = null, string[] keys = null)
            {
                Prefixes = prefixes is null ? string.Empty : string.Join('|', prefixes);
                Suffixes = suffixes is null ? string.Empty : string.Join('|', suffixes);
                Keys = keys is null ? string.Empty : string.Join('|', keys);
                Hash = (Keys + Prefixes + Suffixes).GetHashCode();
            }

            #endregion Public Constructors

            #region Public Fields

            public readonly string Keys = string.Empty;
            public readonly string Prefixes = string.Empty;
            public readonly string Suffixes = string.Empty;

            #endregion Public Fields

            #region Public Methods

            public bool Equals(ResourceCID other) =>
                other is not null &&
                    Keys.Equals(other.Keys) &&
                    Prefixes.Equals(other.Prefixes) &&
                    Suffixes.Equals(other.Suffixes);

            public override bool Equals(object obj) => Equals(obj as ResourceCID);

            public override int GetHashCode() => Hash;

            public override string ToString() => $"{Prefixes} -- {Suffixes} -- {Keys}";

            #endregion Public Methods

            /// <summary>
            /// Load resource kind values from I18n dictionary
            /// </summary>
            /// <param name="prefixes"></param>
            /// <param name="suffixes"></param>
            /// <param name="keys"></param>
            /// <returns></returns>
            public static List<string> Load(string[] prefixes = null, string[] suffixes = null, string[] keys = null)
            {
                List<object> values = [];

                foreach (DictionaryEntry pair in App.I18nDictionary)
                {
                    string key = pair.Key as string;

                    if (keys is not null && keys.Contains(key))
                    {
                        values.Add(pair.Value);
                        continue;
                    }

                    if (prefixes is not null)
                    {
                        foreach (string prefix in prefixes)
                        {
                            if (key.StartsWith(prefix))
                            {
                                values.Add(pair.Value);
                            }
                        }
                    }

                    if (suffixes is not null)
                    {
                        foreach (string suffix in suffixes)
                        {
                            if (key.EndsWith(suffix))
                            {
                                values.Add(pair.Value);
                            }
                        }
                    }
                }

                List<string> stringValues = [];
                foreach (object value in values)
                {
                    if (value is string str)
                    {
                        stringValues.Add(str);
                    }
                    else if (value is IEnumerable collection)
                    {
                        foreach (object item in collection)
                        {
                            stringValues.Add(item.ToString());
                        }
                    }
                }

                return stringValues;
            }
        }

        #endregion Private Classes
    }
}
