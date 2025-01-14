using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Lib.Tools;

namespace LockScreen.DataTypes.Interfaces
{
    /// <summary>
    /// Extended enum type with localization
    /// </summary>
    /// <remarks>
    /// How to use:
    /// - Inherit new class I18nEnumMyEnum : I18nEnum&lt;I18nEnumMyEnum, MyEnum&gt; <br />
    /// - Add to xaml localization files collection with key "I18nEnumMyEnum" <br />
    /// - Use `ObservableCollection` at `I18nEnumMyEnum.Items` and `I18nEnumMyEnum.Find(myEnumValue)`
    /// method to get `I18nEnum` object or .Enum field to get enum from `I18nEnum` object <br />
    /// - Add to language change event and app init stage next call: `I18nEnum.LoadAll()` <br /> Also
    /// use option `DisplayMemberPath="Text"` in xaml collections controls to allow WPF to attach
    /// PropertyChanged handler and update field in UI or use this filed directly in bindings with
    /// `UpdateSourceTrigger=PropertyChanged` option <br /><br /> In general it works very simple:
    /// <br /> Static ctor in `I18nEnum` searches in assembly all inherited classes from `I18nEnum`, saves
    /// this list to array and then call static method `Register()` for each founded type <br />
    /// </remarks>
    /// <typeparam name="TXValue">Inheritted self type</typeparam>
    /// <typeparam name="TEnum"></typeparam>
    public abstract class I18nEnum<TXValue, TEnum>
        : I18nEnum,
        IEquatable<I18nEnum<TXValue, TEnum>>,
        IEquatable<TXValue>,
        IComparable<I18nEnum<TXValue, TEnum>>,
        IComparable<TXValue>,
        ISupportInitialize // This interface is used in xaml after item is filled with values
        where TXValue : I18nEnum<TXValue, TEnum>
        //where TEnum : Enum
    {
        #region Public Constructors

        /// <summary>
        /// Create new I18nEnum instance
        /// </summary>
        public I18nEnum()
        {
        }

        /// <summary>
        /// Create new I18nEnum instance
        /// </summary>
        public I18nEnum(string text) : this()
        {
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Enum value
        /// </summary>
        public TEnum Enum { get; set; }

        /// <summary>
        /// Get items collection for the this type
        /// </summary>
        public static readonly ObservableCollection<TXValue> Items = [];

        #endregion Public Properties

        #region Private Fields

        //private bool disposedValue = false;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// I18n full resource key
        /// </summary>
        public static string ResourceKey { get; private set; } = I18nResourceKey(typeof(TXValue).FullName);

        public static TXValue Find(TEnum value) => Items.FirstOrDefault(x => x.Enum.Equals(value)) is TXValue xvalue ? xvalue : default;

        /// <summary>
        /// Convert enum to UIEnum
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator I18nEnum<TXValue, TEnum>(TEnum type) => Find(type);

        /// <summary>
        /// Convert UIEnum to enum
        /// </summary>
        /// <param name="item"></param>
        public static implicit operator TEnum(I18nEnum<TXValue, TEnum> item) => item.Enum;

        /// <summary>
        /// Load all collections
        /// </summary>
        public static void Load()
        {
            TXValue[] loadedCollection = FindRes<TXValue[]>(ResourceKey);
            if (Items.Count == 0)
            {
                Items.AddRange(loadedCollection);
                return;
            }

            List<TXValue> itemsToAdd = [];

            foreach (TXValue item in loadedCollection)
            {
                if (Items.FirstOrDefault(x => x.Enum.Equals(item.Enum)) is TXValue oldItem)
                {
                    oldItem.Text = item.Text;
                }
                else
                {
                    itemsToAdd.Add(item);
                }
            }

            // Remove missing items (rare case)
            foreach (TXValue item in Items)
            {
                if (loadedCollection.FirstOrDefault(x => x.Enum.Equals(item.Enum)) is null)
                {
                    Items.Remove(item);
                }
            }

            Items.AddRange(itemsToAdd);

            //Items.Clear();
            //Items.AddRange(loadedCollection);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool operator !=(I18nEnum<TXValue, TEnum> data1, I18nEnum<TXValue, TEnum> data2)
        {
            return
                data1 is not I18nEnum<TXValue, TEnum> a ||
                data2 is not I18nEnum<TXValue, TEnum> b ||
                !a.Enum.Equals(b.Enum) &&
                !a.Text.Equals(b.Text); // Test locale text too
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool operator ==(I18nEnum<TXValue, TEnum> data1, I18nEnum<TXValue, TEnum> data2)
        {
            return
                data1 is I18nEnum<TXValue, TEnum> a &&
                data2 is I18nEnum<TXValue, TEnum> b &&
                a.Enum.Equals(b.Enum) &&
                a.Text.Equals(b.Text); // Test locale text too
        }

        /// <summary>
        /// Register I18nEnum type (for now: just save type name to static var)
        /// </summary>
        /// <param name="i18n"></param>
        public static void Register()
        {
            //I18nKey = typeof(TXValue).Name;
        }

        /// <summary>
        /// BeginInit
        /// </summary>
        public void BeginInit()
        {
        }

        /// <summary>
        /// CompareTo
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(TXValue other) => Text.CompareTo(other.Text);

        /// <summary>
        /// CompareTo
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(I18nEnum<TXValue, TEnum> other) => Text.CompareTo(other.Text);

        /// <summary>
        /// EndInit
        /// </summary>
        public void EndInit()
        {
        }

        /// <summary>
        /// Item equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(I18nEnum<TXValue, TEnum> other) => other is not null && Enum.Equals(other.Enum);

        /// <summary>
        /// Item equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TXValue other) => other is not null && Enum.Equals(other.Enum);

        /// <summary>
        /// Object equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other is I18nEnum<TXValue, TEnum> item)
            {
                return Enum.Equals(item.Enum);
            }
            else if (other is TEnum type)
            {
                return Enum.Equals(type);
            }
            else if (other is string str)
            {
                return Ext.IsEnum(str, Enum);
            }
            return false;
        }

        /// <summary>
        /// String enum type equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(string other)
        {
            return Ext.IsEnum(other, Enum);
        }

        /// <summary>
        /// Type equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TEnum other)
        {
            return other is TEnum type && Enum.Equals(type);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 2049151605 + EqualityComparer<TEnum>.Default.GetHashCode(Enum);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Text;

        #endregion Public Methods
    }

    /// <summary>
    /// Base I18nEnum class
    /// </summary>
    public abstract class I18nEnum : II18nEnum, INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        /// I18n localization of this enum value
        /// </summary>
        /// <remarks>
        /// <seealso cref="INotifyPropertyChanged" /> is supported: use DisplayMemberPath="Text" in
        /// xaml for runtime field update
        /// </remarks>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        private string text;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// - Find all inherited not abstract types <br />
        /// - Add them to the RegisteredTypes[] static array <br />
        /// - Call static method Register() for each type
        /// </summary>
        /// <remarks>
        /// Fun fact. <br /> With static ctor call order is next: static IsInitialized = Init()
        /// -&gt; static ctor() -&gt; static void LoadAll() <br /> BUT! If static ctor isn't defined
        /// call order is next: static void LoadAll() -&gt; IsInitialized = Init() WTF??? (╯'□')╯︵ ┻━┻
        /// </remarks>
        static I18nEnum()
        {
            RegisteredTypes = Ext.GetSubclassOf<I18nEnum>().ToArray();
            foreach (Type type in RegisteredTypes)
            {
                MethodInfo register = type.GetMethod(
                    "Register",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                register.Invoke(null, []);
            }
        }

        #endregion Public Constructors

        #region Public Fields

        public static readonly Type[] RegisteredTypes;

        #endregion Public Fields

        #region Public Events

        /// <summary>
        /// All collections is of all types is loaded
        /// </summary>
        public static event EventHandler AllLoaded;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// Call static method "Load()" in each inheritted types
        /// </summary>
        public static void LoadAll()
        {
            foreach (Type type in RegisteredTypes)
            {
                MethodInfo load = type.GetMethod(
                    "Load",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                load.Invoke(null, []);
            }

            AllLoaded?.Invoke(null, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}
