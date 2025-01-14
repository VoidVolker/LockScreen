using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

using LockScreen.DataTypes.Collections.I18n;
using LockScreen.DataTypes.Enums;
using LockScreen.DataTypes.Events;
using LockScreen.DataTypes.Interfaces;
using LockScreen.Models;

using Prism.Mvvm;

using static LockScreen.VM.LangVM;

namespace LockScreen.VM
{
    public class SettingsVM : BindableBase
    {
        #region Public Constructors

        public SettingsVM(AppVM appVM)
        {
            AppVM = appVM;
            InitEvents();
            InitVars();
        }

        #endregion Public Constructors

        #region Public Properties

        public AppVM AppVM { get; private set; }

        /// <summary>
        /// Service state mode
        /// </summary>
        public I18nServiceState LogonServiceState
        {
            get => serviceState;
            set
            {
                serviceState = value;
                serviceControl = value.Enum;
                RaisePropertyChanged(nameof(LogonServiceState));
                RaisePropertyChanged(nameof(ServiceManageEnabled));
                RaisePropertyChanged(nameof(ServiceInstallEnabled));
                if (
                    _serviceState == ServiceState.Stopped ||
                    _serviceState == ServiceState.Running ||
                    _serviceState == ServiceState.Paused
                )
                {
                    RaisePropertyChanged(nameof(ServiceManageContent));
                }
            }
        }

        /// <summary>
        /// Open settings directory
        /// </summary>
        public CommandEvent OpenSettingsDir { get; } = new CommandEvent();

        /// <summary>
        /// Uninstall service
        /// </summary>
        public CommandEvent ServiceInstall { get; } = new CommandEvent();

        /// <summary>
        /// Is service install button enabled
        /// </summary>
        public bool ServiceInstallEnabled
        {
            get => serviceInstallEnabled;
            set
            {
                serviceInstallEnabled = value;
                RaisePropertyChanged(nameof(ServiceInstallEnabled));
                RaisePropertyChanged(nameof(ServiceManageEnabled));
            }
        }

        /// <summary>
        /// Service management
        /// </summary>
        public CommandEvent ServiceManage { get; } = new CommandEvent();

        /// <summary>
        /// Service manage text
        /// </summary>
        public I18nServiceControl ServiceManageContent
        {
            get => serviceControl;
            //set
            //{
            //    RaisePropertyChanged(nameof(ServiceManageContent));
            //}
        }

        /// <summary>
        /// Is service control enabled
        /// </summary>
        public bool ServiceManageEnabled
        {
            get =>
                serviceInstallEnabled &&
                (_serviceState == ServiceState.Stopped ||
                _serviceState == ServiceState.Running ||
                _serviceState == ServiceState.Paused);
        }

        /// <summary>
        /// Config file location
        /// </summary>
        public string SettingsFileLocation
        {
            get => System.IO.Path.GetDirectoryName(AppVM.App.Settings.FilePath);
        }

        private bool serviceInstallEnabled = true;

        #endregion Public Properties

        #region Private Properties

        private void SetServiceStatus(ServiceState value)
        {
            _serviceState = value;
            LogonServiceState = value;
        }

        #endregion Private Properties

        #region Private Fields

        private ServiceState _serviceState = ServiceState.None;
        private I18nServiceControl serviceControl = null;
        private I18nServiceState serviceState = null;

        #endregion Private Fields

        #region Private Methods

        private static bool ServiceInstallQestion(ServiceState state) =>
            ServiceQuestion(
                I18n(
                    state == ServiceState.NotInstalled
                    ? "Settings Service install button"
                    : "Settings Service uninstall button"
                ));

        private static bool ServiceManageQestion(ServiceState state) =>
            ServiceQuestion(I18nServiceControl.Find(state).Text);

        private static bool ServiceQuestion(string operation)
        {
            operation = operation.ToLower();
            string message = string.Format(I18n("Service Are you sure message"), operation);
            string caption = string.Format(I18n("Service Are you sure caption"), operation);
            return AppVM.MessageBoxQuestion(message, caption) == MessageBoxResult.Yes;
        }

        private void InitEvents()
        {
            // UI user events
            OpenSettingsDir.Event += OpenSettingsDirClick;
            ServiceManage.Event += ServiceManageClick;
            ServiceInstall.Event += ServiceInstallClick;

            // Internal events
            I18nEnum.AllLoaded += XEnum_AllLoaded;
        }

        private void InitVars()
        {
            // Try find and apply locale from config
            if (AppVM.LangVM.TryFind(AppVM.App.Settings.Locale) is Lang lang)
            {
                AppVM.LangVM.Language = lang;
            }
            else // Reset locale in config
            {
                AppVM.App.Settings.Locale = AppVM.LangVM.Language.Culture;
                AppVM.App.Settings.Save();
                AppVM.ConfigSaved = DateTime.Now;
            }

            try
            {
                ServiceInstallEnabled = false;
                LogonServiceApp ls = new();
                SetServiceStatus(ls.GetStatus());
                ServiceInstallEnabled = true;
            }
            catch (Exception e)
            {
                AppVM.MessageBoxError(e.Message);
            }
        }

        private void OpenSettingsDirClick(object sender, EventArgs e)
        {
            string location = System.IO.Path.GetDirectoryName(AppVM.App.Settings.FilePath);
            Process.Start("explorer.exe", location);
        }

        private void ServiceInstallClick(object sender, EventArgs e)
        {
            switch (_serviceState)
            {
                // Install service
                case ServiceState.NotInstalled:
                    if (ServiceInstallQestion(_serviceState))
                    {
                        Task.Run(() =>
                        {
                            ServiceInstallEnabled = false;
                            SetServiceStatus(ServiceState.Installing);
                            LogonServiceApp ls = new();
                            ls.InstallCommand();
                            SetServiceStatus(ls.GetStatus());
                            ServiceInstallEnabled = true;
                        });
                    }
                    break;

                // Uninstall service
                default:
                    if (ServiceInstallQestion(_serviceState))
                    {
                        Task.Run(() =>
                        {
                            ServiceInstallEnabled = false;
                            SetServiceStatus(ServiceState.Uninstalling);
                            LogonServiceApp ls = new();
                            ls.UninstallCommand();
                            SetServiceStatus(ls.GetStatus());
                            ServiceInstallEnabled = true;
                        });
                    }
                    break;
            }
        }

        private void ServiceManageClick(object sender, EventArgs e)
        {
            switch (_serviceState)
            {
                // Start service
                case ServiceState.Paused:
                case ServiceState.Stopped:
                    if (ServiceManageQestion(_serviceState))
                    {
                        Task.Run(() =>
                        {
                            ServiceInstallEnabled = false;
                            SetServiceStatus(ServiceState.StartPending);
                            LogonServiceApp ls = new();
                            ls.StartCommand();
                            SetServiceStatus(ls.GetStatus());
                            ServiceInstallEnabled = true;
                        });
                    }
                    break;

                // Stop service
                case ServiceState.Running:
                    if (ServiceManageQestion(_serviceState))
                    {
                        Task.Run(() =>
                        {
                            ServiceInstallEnabled = false;
                            SetServiceStatus(ServiceState.StopPending);
                            LogonServiceApp ls = new();
                            ls.StopCommand();
                            SetServiceStatus(ls.GetStatus());
                            ServiceInstallEnabled = true;
                        });
                    }
                    break;
            }
        }

        private void XEnum_AllLoaded(object sender, EventArgs e)
        {
            SetServiceStatus(_serviceState);
        }

        #endregion Private Methods
    }
}
