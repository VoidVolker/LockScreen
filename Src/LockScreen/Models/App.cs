using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;

using Lib.DataTypes.Enums;
using Lib.DataTypes.Structures;
using Lib.Tools;

using LockScreen.Tools;
using LockScreen.VM;

namespace LockScreen.Models
{
    public class App
    {
        public App()
        {
            Settings = new LocalSettings(Info.WorkFile(LocalSettings.FileName));
            Settings.Load();
            SettingsValidate();
        }

        public List<Screen> Screens = [];

        /// <summary>
        /// Settings
        /// </summary>
        public LocalSettings Settings;

        //private const string LogonServiceProcessName = "LogonService";
        private const string LogonServiceName = "LogonService";

        public static void InstallService()
        {
        }

        /// <summary>
        /// Service installation status
        /// </summary>
        /// <returns></returns>
        public static bool IsInstalled() =>
            ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == LogonServiceName) != null;

        public static void UninstallService()
        {
        }

        private void SettingsValidate()
        {
            if (Settings.WallpaperMode == WallpaperMode.One)
            {
                if (!string.IsNullOrEmpty(Settings.Wallpaper) && !File.Exists(Settings.Wallpaper))
                {
                    AppVM.MessageBoxError(I18n("Wallpaper not found"), Settings.Wallpaper);
                }
            }
            else
            {
                string notFoundErr = I18n("Wallpaper not found");
                foreach (Screen screen in Settings.Screens)
                {
                    if (string.IsNullOrEmpty(screen.Wallpaper)) { continue; }
                    if (File.Exists(screen.Wallpaper)) { continue; }
                    AppVM.MessageBoxError(notFoundErr, screen.Wallpaper);
                }
            }
        }
    }
}
