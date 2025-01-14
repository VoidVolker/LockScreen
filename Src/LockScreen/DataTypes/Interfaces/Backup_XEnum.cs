//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;

//using Lib.Tools;

//namespace LockScreen.DataTypes.Interfaces
//{
//    /// <summary>
//    /// Extended enum with localization
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public abstract class XEnum<XT, T>
//        : XEnum,
//        IEquatable<XEnum<XT, T>>,
//        IEquatable<XT>,
//        IComparable<XEnum<XT, T>>,
//        IComparable<XT>,
//        IDisposable,
//        ISupportInitialize // Key feature used in xaml after item in defined with values
//        where XT : XEnum<XT, T>
//        where T : Enum
//    {
//        #region Public Constructors

// ///
// <summary>
// /// Create new XEnum instance ///
// </summary>
// public XEnum() { Collection = GetCollection(); }

// ///
// <summary>
// /// Create new XEnum instance ///
// </summary>
// public XEnum(string text) : this() { Text = text; }

// #endregion Public Constructors

// #region Public Properties

// /// <summary> /// Get items collection for the this type /// </summary> public static
// ObservableCollection<XT> Items => Collections[typeof(XT)];

// ///
// <summary>
// /// Enum value ///
// </summary>
// public T Enum { get; set; }

// #endregion Public Properties

// #region Public Fields

// /// <summary> /// Typed items list with collections descriptors /// </summary> public static
// readonly Dictionary<Type, XEnumCollection<XT, T>> Collections = [];

// /// <summary> /// XEnum values collection /// </summary> public readonly XEnumCollection<XT, T> Collection;

// #endregion Public Fields

// #region Private Fields

// private bool disposedValue = false;

// #endregion Private Fields

// #region Public Methods

// public static XT Find(T value) => Collections[typeof(XT)].Find(value);

// //{ // var collection = Collections[typeof(XT)]; // return collection.Find(value); // //return
// collection.Items.FirstOrDefault(x => x.Enum.Equals(value)) is XT xValue ? xValue : null; //}

// /// <summary> /// Convert UIEnum to enum /// </summary> /// <param name="item"></param> public
// static implicit operator T(XEnum<XT, T> item) => item.Enum;

// /// <summary> /// Convert enum to UIEnum /// </summary> /// <param name="type"></param> public
// static implicit operator XEnum<XT, T>(T type) => Find(type);

// ///
// <summary>
// /// Load all collections ///
// </summary>
// public static void Load() { Collections.Each((t, c) =&gt; { c.Load(); }); }

// /// <summary> /// != /// </summary> /// <param name="data1"></param> /// <param
// name="data2"></param> /// <returns></returns> public static bool operator !=(XEnum<XT, T> data1,
// XEnum<XT, T> data2) { return data1 is not XEnum<XT, T> a || data2 is not XEnum<XT, T> b ||
// !a.Enum.Equals(b.Enum) && !a.Text.Equals(b.Text); // Test locale text too }

// /// <summary> /// == /// </summary> /// <param name="data1"></param> /// <param
// name="data2"></param> /// <returns></returns> public static bool operator ==(XEnum<XT, T> data1,
// XEnum<XT, T> data2) { return data1 is XEnum<XT, T> a && data2 is XEnum<XT, T> b &&
// a.Enum.Equals(b.Enum) && a.Text.Equals(b.Text); // Check locale text too }

// /// <summary> /// Register XEnum type /// </summary> /// <param name="i18n"></param> public
// static void Register() { Type type = typeof(XT); string i18n = type.Name; XEnumCollection<XT, T>
// collection = new(i18n); Collections[type] = collection; }

// ///
// <summary>
// /// BeginInit ///
// </summary>
// public void BeginInit() { }

// ///
// <summary>
// /// CompareTo ///
// </summary>
// ///
// <param name="other"></param>
// ///
// <returns></returns>
// public int CompareTo(XT other) =&gt; Text.CompareTo(other.Text);

// /// <summary> /// CompareTo /// </summary> /// <param name="other"></param> ///
// <returns></returns> public int CompareTo(XEnum<XT, T> other) => Text.CompareTo(other.Text);

// // Этот код добавлен для правильной реализации шаблона высвобождаемого класса. ///
// <summary>
// /// Dispose ///
// </summary>
// public void Dispose() { // Не изменяйте этот код. Разместите код очистки выше, в методе
// Dispose(bool disposing). //Dispose(true); // TODO: раскомментировать следующую строку, если метод
// завершения переопределен выше. GC.SuppressFinalize(this); GC.SuppressFinalize(this); }

// ///
// <summary>
// /// EndInit ///
// </summary>
// public void EndInit() { //Collection.Remove((XT)this); //if (Collection.FirstOrDefault(x =&gt;
// x.Equals(this)) is XT existing) //{ // existing.Text = Text; //} //else //{ //
// Collection.Add((XT)this); //} }

// // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код
// для освобождения неуправляемых ресурсов. // ~UIData() { // Не изменяйте этот код. Разместите код
// очистки выше, в методе Dispose(bool // disposing). Dispose(false); } /// <summary> /// Item
// equals /// </summary> /// <param name="other"></param> /// <returns></returns> public bool
// Equals(XEnum<XT, T> other) => other is not null && Enum.Equals(other.Enum);

// /// <summary> /// Item equals /// </summary> /// <param name="other"></param> ///
// <returns></returns> public bool Equals(XT other) => other is not null && Enum.Equals(other.Enum);

// /// <summary> /// Object equals /// </summary> /// <param name="other"></param> ///
// <returns></returns> public override bool Equals(object other) { if (other is XEnum<XT, T> item) {
// return Enum.Equals(item.Enum); } else if (other is T type) { return Enum.Equals(type); } else if
// (other is string str) { return Ext.IsEnum(str, Enum); } return false; }

// ///
// <summary>
// /// String enum type equals ///
// </summary>
// ///
// <param name="other"></param>
// ///
// <returns></returns>
// public bool Equals(string other) { return Ext.IsEnum(other, Enum); }

// /// <summary> /// Type equals /// </summary> /// <param name="other"></param> ///
// <returns></returns> public bool Equals(T other) { return other is T type && Enum.Equals(type); }

// public XEnumCollection<XT, T> GetCollection() => Collections[typeof(XT)];

// /// <summary> /// GetHashCode /// </summary> /// <returns></returns> public override int
// GetHashCode() { return 2049151605 + EqualityComparer<T>.Default.GetHashCode(Enum); }

// ///
// <summary>
// /// ToString ///
// </summary>
// ///
// <returns></returns>
// public override string ToString() =&gt; Text;

// #endregion Public Methods

// #region Protected Methods

// // Для определения избыточных вызовов ///
// <summary>
// /// Dispose ///
// </summary>
// ///
// <param name="disposing"></param>
// protected virtual void Dispose(bool disposing) { if (!disposedValue) { if (disposing) { // TODO:
// освободить управляемое состояние (управляемые объекты). Collection.Remove((XT)this); }

// // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод
// завершения. // TODO: задать большим полям значение NULL.

// disposedValue = true; } }

// #endregion Protected Methods }

// public abstract class XEnum : IXEnum { #region Public Properties

// public string Text { get; set; }

// #endregion Public Properties

// #region Public Constructors

// /// <summary> /// </summary> /// <remarks> /// Fun fact. <br /> With static ctor call order is
// next: static IsInitialized = Init() /// -&gt; static ctor() -&gt; static void LoadAll() <br />
// BUT! If static ctor isn't defined /// call order is next: static void LoadAll() -&gt;
// IsInitialized = Init() WTF??? (╯'□')╯︵ ┻━┻ /// </remarks> static XEnum() { RegisteredTypes =
// Ext.GetSubclassOf<XEnum>().ToList(); foreach (Type type in RegisteredTypes) { var register =
// type.GetMethod( "Register", BindingFlags.Static | BindingFlags.Public |
// BindingFlags.FlattenHierarchy); register.Invoke(null, []); } }

// #endregion Public Constructors

// #region Public Fields

// public static readonly List<Type> RegisteredTypes;

// #endregion Public Fields

// #region Public Events

// ///
// <summary>
// /// All collections is of all types is loaded ///
// </summary>
// public static event EventHandler AllLoaded;

// #endregion Public Events

// #region Public Methods

// public static void LoadAll() { //LoadEvent?.Invoke(null, EventArgs.Empty); foreach (Type type in
// RegisteredTypes) { var load = type.GetMethod( "Load", BindingFlags.Static | BindingFlags.Public |
// BindingFlags.FlattenHierarchy); load.Invoke(null, []); }

// AllLoaded?.Invoke(null, EventArgs.Empty); }

// #endregion Public Methods

// /////
// <summary>
// ///// Add on load event handler /////
// </summary>
// /////
// <param name="action"></param>
// //public static void OnLoad(Action action) =&gt; LoadEvent += (s, e) =&gt; action(); /////
// <summary>
// ///// Resources load event /////
// </summary>
// //public static event EventHandler LoadEvent; }

// public class XEnumCollection<XT, T> /*: IEquatable<XEnumCollection<T>>, IEquatable<Type>*/ :
// ObservableCollection<XT> where XT : XEnum<XT, T> where T : Enum { #region Public Constructors

// public XEnumCollection() { }

// public XEnumCollection(string stringId) : this() { StringId = stringId; }

// #endregion Public Constructors

// ///// <summary> ///// XEnum values collection ///// </summary> //public readonly
// ObservableCollection<XT> Items = [];

// #region Public Fields

// ///
// <summary>
// /// Localization ID ///
// </summary>
// public readonly string StringId;

// #endregion Public Fields

// #region Private Fields

// private NotifyCollectionChangedEventHandler collectionChanged;

// #endregion Private Fields

// #region Public Events

// public override event NotifyCollectionChangedEventHandler CollectionChanged { add =>
// collectionChanged += value; remove => collectionChanged -= value; }

// #endregion Public Events

// #region Public Methods

// public XT Find(T value) { return Items.FirstOrDefault(x => x.Enum.Equals(value)) is XT xvalue ?
// xvalue : null; }

// /// <summary> /// Load this type localization collection /// </summary> public void Load() { XT[]
// loadedCollection = I18n<XT[]>(StringId);

// // Just copy data from new item to old item foreach (XT item in loadedCollection) { if
// (Items.FirstOrDefault(x => x.Enum.Equals(item.Enum)) is XT oldItem) { oldItem.Text = item.Text; }
// else { Items.Add(item); } }

// // Remove missing items (rare case) foreach (XT item in Items) { if
// (loadedCollection.FirstOrDefault(x => x.Enum.Equals(item.Enum)) is null) { Items.Remove(item); } }

// //Clear(); //this.AddRange(loadedCollection);

// collectionChanged?.Invoke( this, new
// NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); }

//        #endregion Public Methods
//    }
//}
