using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;

namespace LockScreen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Public Constructors

        public App() : base()
        {
            InitializeComponent();

            Languages.Clear();
            foreach (string locale in LoadLocales())
            {
                Languages.Add(new CultureInfo(locale));
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Current I18n resources dictionary
        /// </summary>
        public static ResourceDictionary I18nDictionary { get; private set; } = I18nDictionaryGet();

        /// <summary>
        /// Application current language
        /// </summary>
        public static CultureInfo Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value == Thread.CurrentThread.CurrentUICulture) return;

                //1. Switching application language
                Thread.CurrentThread.CurrentUICulture = value;

                //2. Create ResourceDictionary for new culture
                ResourceDictionary dict = new()
                {
                    Source = new Uri($"{langResourcesDir}{value.Name}.xaml", UriKind.Relative)
                };

                //3. Find old ResourceDictionary and remove it, then add new ResourceDictionary
                Collection<ResourceDictionary> Res = Current.Resources.MergedDictionaries;
                //ResourceDictionary oldDict = I18nDictionaryGet();

                if (I18nDictionary == null)
                {
                    Res.Add(dict);
                }
                else
                {
                    int oldIndex = Res.IndexOf(I18nDictionary);
                    Res.Remove(I18nDictionary);
                    Res.Insert(oldIndex, dict);
                }

                I18nDictionary = dict;

                //4. Trigger LanguageChanged event
                LanguageChanged?.Invoke(Current, new EventArgs());
            }
        }

        /// <summary>
        /// Current application language
        /// </summary>
        public static List<CultureInfo> Languages { get; } = [];

        private static ResourceDictionary I18nDictionaryGet() =>
            Current.Resources.MergedDictionaries.First(
                (d) => d.Source != null && d.Source.OriginalString.StartsWith(langResourcesDir)
            );

        #endregion Public Properties

        #region Private Fields

        private const string langResourcesDir = "Resources/Lang/";

        // "ru-RU".Length -> 5
        private const int localeLen = 5;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Language changed event
        /// </summary>
        public static event EventHandler LanguageChanged;

        #endregion Public Events

        #region Private Methods

        private static List<string> LoadLocales()
        {
            List<string> localesList = [];
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources");
            ResourceReader resourceReader = new(stream);

            foreach (DictionaryEntry resource in resourceReader)
            {
                string respath = resource.Key.ToString();
                if (respath.StartsWith(langResourcesDir, StringComparison.OrdinalIgnoreCase))
                {
                    string locale = respath.Substring(langResourcesDir.Length, localeLen);
                    localesList.Add(locale);
                }
            }
            return localesList;
        }

        #endregion Private Methods
    }
}
