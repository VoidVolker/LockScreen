using System;
using System.Windows;

using LockScreen.DataTypes.Interfaces;

using Prism.Mvvm;

namespace LockScreen.VM
{
    /// <summary>
    /// AppVM
    /// </summary>
    public class AppVM : BindableBase
    {
        #region Public Constructors

        public AppVM()
        {
            InitLang();
            InitApp();
            AboutVM = new AboutVM();
            SettingsVM = new SettingsVM(this);
            LockScreenVM = new LockScreenVM(this);
            MainWinInit();

            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// About tab VM
        /// </summary>
        public AboutVM AboutVM { get; }

        /// <summary>
        /// Config save time
        /// </summary>
        public DateTime ConfigSaved
        {
            get => configSaved;
            set
            {
                configSaved = value;
                RaisePropertyChanged(nameof(ConfigSaved));
            }
        }

        /// <summary>
        /// Language control VM
        /// </summary>
        public LangVM LangVM { get; private set; }

        /// <summary>
        /// Lock Screen tab VM
        /// </summary>
        public LockScreenVM LockScreenVM { get; }

        /// <summary>
        /// Settings tab VM
        /// </summary>
        public SettingsVM SettingsVM { get; }

        #endregion Public Properties

        #region Internal Properties

        // Models
        internal Models.App App { get; private set; }

        #endregion Internal Properties

        #region Private Fields

        private DateTime configSaved;

        #endregion Private Fields

        #region Public Methods

        public static void MessageBoxError(string message)
        {
            MessageBox.Show(
                message,
                I18n("Error caption"),
                MessageBoxButton.OK,
            MessageBoxImage.Error
            );
        }

        public static void MessageBoxError(string message, string errorText) =>
            MessageBoxError($"{message}: {errorText}");

        public static MessageBoxResult MessageBoxQuestion(string message, string caption) =>
            MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No
            );

        #endregion Public Methods

        #region Private Methods

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            try
            {
                I18nEnum.LoadAll();
                App.Settings.Locale = LangVM.Language.Culture;
                App.Settings.Save();
                ConfigSaved = DateTime.Now;
            }
            catch (Exception err)
            {
                MessageBoxError(I18n("Settings save error"), err.Message);
            }
        }

        private void InitApp()
        {
            try
            {
                App = new Models.App();
            }
            catch (Exception e)
            {
                MessageBoxError(I18n("App core init error"), e.Message);
            }
        }

        private void InitLang()
        {
            LockScreen.App.LanguageChanged += App_LanguageChanged;
            LangVM = new LangVM();
            I18nEnum.LoadAll();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LockScreenVM.Preview.CloseAll();
        }

        private void MainWinInit()
        {
            LockScreenVM.ScreensLoad();
        }

        #endregion Private Methods
    }
}
