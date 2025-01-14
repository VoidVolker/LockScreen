using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using Lib.DataTypes.Structures;

using Lib.Tools;

using LockScreen.DataTypes.Collections.I18n;
using LockScreen.DataTypes.Commands;
using LockScreen.DataTypes.Events;
using LockScreen.Models;
using LockScreen.Views.Controls;

using Prism.Mvvm;

namespace LockScreen.VM
{
    public class LockScreenVM : BindableBase
    {
        #region Public Constructors

        public LockScreenVM(AppVM appVM)
        {
            AppVM = appVM;
            //TypesLoad();
            InitEvents();
            InitVars();
        }

        #endregion Public Constructors

        #region Public Properties

        public AppVM AppVM { get; private set; }

        /// <summary>
        /// Supported images formats list
        /// </summary>
        public string[] ExtList
        {
            get => extList;
            set
            {
                RaisePropertyChanged(nameof(ExtList));
            }
        }

        /// <summary>
        /// Is all screens preview open
        /// </summary>
        public bool IsPreviewOpened
        {
            get => isPreviewOpened;
            set
            {
                if (isPreviewOpened == value) { return; }
                isPreviewOpened = value;
                RaisePropertyChanged(nameof(IsPreviewOpened));
                RaisePropertyChanged(nameof(PreviewOperation));
            }
        }

        /// <summary>
        /// Current preview operation localization
        /// </summary>
        public I18nPreviewOperation PreviewOperation
        {
            get => isPreviewOpened;
        }

        /// <summary>
        /// Preview open/close command
        /// </summary>
        public CommandEvent PreviewSwitch { get; } = new CommandEvent();

        /// <summary>
        /// Delete selected disconnected screen
        /// </summary>
        public ScreenCommand ScreenDelete { get; } = new();

        /// <summary>
        /// Preview wallpaper for selected screen
        /// </summary>
        public ScreenCommand ScreenDetect { get; } = new();

        public ScreenCommand ScreenPreviewSwitch { get; } = new();

        /// <summary>
        /// Screens list
        /// </summary>
        public ObservableCollection<ScreenVM> Screens
        {
            get => screens;
            set
            {
                screens = value;
                RaisePropertyChanged(nameof(Screens));
            }
        }

        /// <summary>
        /// Screens count
        /// </summary>
        public ushort ScreensCount
        {
            get => screensCount;
            set
            {
                if (screensCount == value) { return; }
                screensCount = value;
                RaisePropertyChanged(nameof(ScreensCount));
            }
        }

        /// <summary>
        /// Update screens list
        /// </summary>
        public CommandEvent ScreensUpdate { get; } = new CommandEvent();

        /// <summary>
        /// Save screen wallpaper
        /// </summary>
        public ScreenCommand ScreenWallpaperSave { get; } = new();

        /// <summary>
        /// Path to one wallpaper file
        /// </summary>
        public string Wallpaper
        {
            get => AppVM.App.Settings.Wallpaper;
            set
            {
                if (AppVM.App.Settings.Wallpaper == value || value is null) { return; }
                // Save wallpaper to settings
                AppVM.App.Settings.Wallpaper = value;
                AppVM.App.Settings.Save();
                AppVM.ConfigSaved = DateTime.Now;

                RaisePropertyChanged(nameof(Wallpaper));
            }
        }

        /// <summary>
        /// Wallpaper mode
        /// </summary>
        public I18nWallpaperMode WallpaperMode
        {
            get => wallpaperMode;
            set
            {
                if (wallpaperMode == value || value is null) { return; }
                wallpaperMode = value;

                // Save mode to settings
                AppVM.App.Settings.WallpaperMode = value;
                AppVM.App.Settings.Save();
                AppVM.ConfigSaved = DateTime.Now;

                Preview.CloseAll();

                RaisePropertyChanged(nameof(WallpaperMode));
            }
        }

        /// <summary>
        /// Mode types list
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Пометьте члены как статические", Justification = "<Ожидание>")]
        [SuppressMessage("CodeQuality", "IDE0079:Удалить ненужное подавление", Justification = "<Ожидание>")]
        public ObservableCollection<I18nWallpaperMode> WallpaperModes
        {
            get => I18nWallpaperMode.Items;
        }

        #endregion Public Properties

        #region Public Fields

        public readonly PreviewManager Preview = new();

        #endregion Public Fields

        #region Private Fields

        //// See BitmapImageCheck().NativeSupportedExtensions to get this list in runtime
        //// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/imaging-overview#wpf-image-formats
        //// https://learn.microsoft.com/en-us/windows/win32/wic/-wic-about-windows-imaging-codec
        //// Static list of WPF supported formats
        //private static readonly List<string> extList =
        //    [
        //        ".bmp", ".dds", ".gif", ".hdp", ".ico", ".jpeg", ".jpg", ".jxr", ".png", ".tiff", ".wdp"
        //    ];

        // Static list of WindowsForms supported formats
        private static readonly string[] extList = [".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tiff"];

        private bool isPreviewOpened = false;
        private ObservableCollection<ScreenVM> screens = [];
        private ushort screensCount = 1;
        private I18nWallpaperMode wallpaperMode = null;
        private ObservableCollection<I18nWallpaperMode> wallpaperModes = [];

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Load/reload screens collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ScreensLoad(object sender = null, EventArgs e = null)
        {
            List<Screen> newScreens = AppVM.App.Settings.ScreensUpdate();
            List<ScreenVM> newVms = newScreens.Select(ScreenVM.From).ToList();

            // Set preview flag
            newVms.Each(vm => vm.IsPreviewOpened = Preview.IsOpened(vm.Screen.Id));

            // Close preview for disconnected screens
            newVms.Where(vm => vm.IsPreviewOpened && vm.Screen.IsDiconnected)
                .Each(vm => Preview.Close(vm.Screen.Id));

            // Save updated screens in config file
            AppVM.App.Settings.Screens = newScreens;
            AppVM.App.Settings.Save();
            AppVM.ConfigSaved = DateTime.Now;

            Screens.Clear();
            Screens.AddRange(newVms);
        }

        #endregion Public Methods

        #region Private Methods

        private void InitEvents()
        {
            // Internal events
            App.LanguageChanged += XEnumAllLoaded;
            ScreensUpdate.Event += ScreensLoad;
            Screens.CollectionChanged += ScreensCollectionChanged;

            // UI user events
            PreviewSwitch.Event += PreviewSwitchClick;

            // Screens list control - UI user events
            ScreenWallpaperSave.Event += ScreenWallpaperSaveEvent;
            ScreenPreviewSwitch.Event += ScreenPreviewSwitchClick;
            ScreenDelete.Event += ScreenDeleteClick;
            ScreenDetect.Event += ScreenDetectClick;

            // Preview control events
            Preview.Opened += Preview_Opened;
            Preview.Closed += Preview_Closed;
        }

        private void InitVars()
        {
            WallpaperMode = AppVM.App.Settings.WallpaperMode;
            ExtList = extList;
        }

        private void Preview_Closed(object sender, WallpaperApp app)
        {
            Screens
                .Where(s => s.Screen.Id == app.ScreenId)
                .Each(s => s.IsPreviewOpened = false);

            if (Screens.All(s => !s.IsPreviewOpened))
            {
                IsPreviewOpened = false;
            };
        }

        private void Preview_Opened(object sender, WallpaperApp app)
        {
            Screens
                .Where(s => s.Screen.Id == app.ScreenId)
                .Each(s => s.IsPreviewOpened = true);

            if (Screens.All(s => !s.IsPreviewOpened))
            {
                IsPreviewOpened = false;
            };
        }

        private void PreviewCloseClick()
        {
            Preview.CloseAll();
            IsPreviewOpened = false;
        }

        private void PreviewOpenClick()
        {
            IsPreviewOpened = true;

            string wallpaper = AppVM.App.Settings.Wallpaper;
            AppVM.App.Settings.Screens
                .Where(s => s.IsSecondary)
                .Each(s => Preview.Open(s.Id, wallpaper));
        }

        private void PreviewSwitchClick(object sender, EventArgs e)
        {
            if (IsPreviewOpened)
            {
                PreviewCloseClick();
            }
            else
            {
                PreviewOpenClick();
            }
        }

        private void ScreenDeleteClick(object sender, ScreenEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                I18n("Settings Screen delete question"),
                I18n("Settings Screen delete question title"),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No
            );
            if (result == MessageBoxResult.Yes)
            {
                Screens.RemoveAt(e.Screen.Index);
                Preview.Close(e.VM.Screen.Id);
                AppVM.App.Settings.Screens.Remove(e.Screen);
                AppVM.App.Settings.Save();
                AppVM.ConfigSaved = DateTime.Now;
            }
        }

        private void ScreenDetectClick(object sender, ScreenEventArgs e)
        {
            Preview.FindApp(e.Screen.Id);
        }

        private void ScreenPreviewSwitchClick(object sender, ScreenEventArgs e)
        {
            ScreenVM vm = e.VM;
            Screen screen = vm.Screen;
            if (vm.IsPreviewOpened)
            {
                Preview.Close(screen.Id);
            }
            else
            {
                Preview.Open(screen.Id, screen.Wallpaper);
            }
        }

        // Update screens counter
        private void ScreensCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScreensCount = (ushort)screens.Count;
        }

        private void ScreenWallpaperSaveEvent(object sender, ScreenEventArgs e)
        {
            AppVM.App.Settings.Save();
            AppVM.ConfigSaved = DateTime.Now;
        }

        private void XEnumAllLoaded(object sender, EventArgs e)
        {
            // Update wallpaper modes
            RaisePropertyChanged(nameof(WallpaperMode));
            RaisePropertyChanged(nameof(WallpaperModes));
        }

        #endregion Private Methods
    }
}
