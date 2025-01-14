using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Policy;

using Prism.Mvvm;

namespace LockScreen.VM
{
    /// <summary>
    /// Language control VM
    /// </summary>
    public class LangVM : BindableBase
    {
        //private static readonly NLog.Logger Log = Logging.Get(typeof(LangVM));

        #region Public Constructors

        /// <summary>
        /// Language control VM
        /// </summary>
        public LangVM()
        {
            Languages = new ObservableCollection<Lang>(
                App.Languages.ConvertAll(
                    (c) => new Lang(c)
                )
            );
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Selected Language
        /// </summary>
        public Lang Language
        {
            get => new(App.Language);
            set
            {
                if (value == null) { return; }
                if (App.Language.Equals(value.Culture)) { return; }
                App.Language = value.Culture;
                RaisePropertyChanged(nameof(Language));
            }
        }

        /// <summary>
        /// Language values list
        /// </summary>
        public ObservableCollection<Lang> Languages
        {
            get => _Languages;
            set
            {
                _Languages = value;
                RaisePropertyChanged(nameof(Languages));
            }
        }

        #endregion Public Properties

        #region Private Fields

        private ObservableCollection<Lang> _Languages;

        #endregion Private Fields

        #region Public Methods

        public Lang TryFind(CultureInfo culture)
        {
            foreach (Lang l in Languages)
            {
                if (l.Culture.Equals(culture))
                {
                    return l;
                }
            }
            return null;
        }

        #endregion Public Methods

        //private EventHandler<AppErrorEventArgs> OnVMError;
        ///// <summary>
        ///// Error event
        ///// </summary>
        //public event EventHandler<AppErrorEventArgs> VMError
        //{
        //    add => OnVMError += value;
        //    remove => OnVMError -= value;
        //}

        #region Public Classes

        /// <summary>
        /// Lang structure. Require for rendering english and local culture name.
        /// </summary>
        /// <remarks>
        /// Create lang from culture
        /// </remarks>
        /// <param name="culture"></param>
        public class Lang(CultureInfo culture)
        {
            #region Public Properties

            /// <summary>
            /// Language culture
            /// </summary>
            public CultureInfo Culture { get; set; } = culture;

            /// <summary>
            /// Full language title
            /// </summary>
            public string FullName { get; set; } = culture.EnglishName + " / " + culture.NativeName;

            #endregion Public Properties

            #region Public Methods

            /// <summary>
            /// Equals to object
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj is Lang lang)
                {
                    return Culture.LCID == lang.Culture.LCID;
                }
                else if (obj is CultureInfo culture)
                {
                    return Culture.LCID == culture.LCID;
                }
                return Culture.Equals(obj);
            }

            /// <summary>
            /// GetHashCode
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return 213191193 + EqualityComparer<CultureInfo>.Default.GetHashCode(Culture);
            }

            /// <summary>
            /// To string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return FullName;
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}
