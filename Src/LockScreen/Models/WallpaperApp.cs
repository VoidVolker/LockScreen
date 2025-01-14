using System.Diagnostics;
using System.IO;

using LockScreen.Tools;

namespace LockScreen.Models
{
    public class WallpaperApp : Process
    {
        #region Public Constructors

        public WallpaperApp(string screenId, string wallpaper)
        {
            ScreenId = screenId;
            Wallpaper = wallpaper;
            StartInfo.FileName = FileName;
            StartInfo.WorkingDirectory = Info.AppDirectory;
            StartInfo.Arguments = $"{screenId} \"{Wallpaper}\"";
            EnableRaisingEvents = true;
            Start();
        }

        public WallpaperApp(string screenId)
        {
            ScreenId = screenId;
            StartInfo.FileName = FileName;
            StartInfo.WorkingDirectory = Info.AppDirectory;
            StartInfo.Arguments = screenId;
            EnableRaisingEvents = true;
            Start();
        }

        #endregion Public Constructors

        #region Public Fields

        /// <summary>
        /// Wallpaper application executable full path
        /// </summary>
        public static readonly string FullPath = Path.Combine(Info.AppDirectory, FileName);

        public readonly string ScreenId;
        public readonly string Wallpaper;

        #endregion Public Fields

        #region Private Fields

        private const string FileName = "Wallpaper.exe";

        #endregion Private Fields
    }
}
