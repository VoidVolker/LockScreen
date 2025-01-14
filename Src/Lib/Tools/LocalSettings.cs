using System.Collections.Generic;
using System.Globalization;

using static Lib.Tools.Ext;
using Lib.DataTypes.Enums;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using Lib.DataTypes.Structures;
using System.Linq;
using WindowsDisplayAPI.DisplayConfig;
using WindowsDisplayAPI;

namespace Lib.Tools
{
    /// <summary>
    /// Local settings
    /// </summary>
    /// <param name="filePath"></param>
    public class LocalSettings(string filePath) : JSONFile(filePath)
    {
        #region Public Constructors

        public LocalSettings() : this(FileName)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Locale language settings
        /// </summary>
        [J, Copy] public CultureInfo Locale { get; set; } = CultureInfo.GetCultureInfo(defaultLocale);

        /// <summary>
        /// Individual monitors wallpappers
        /// </summary>
        ///
        [J, Copy] public List<Screen> Screens { get; set; } = [];

        /// <summary>
        /// Path to one wallpaper file
        /// </summary>
        [J, Copy] public string Wallpaper { get; set; } = string.Empty;

        /// <summary>
        /// Wallpaper mode: one wallpaper for all monitors or individual wallpaper for each
        /// </summary>
        [J, Copy] public WallpaperMode WallpaperMode { get; set; } = WallpaperMode.One;

        #endregion Public Properties

        #region Public Fields

        public const string FileName = "settings.json";

        #endregion Public Fields

        #region Private Fields

        private const string defaultLocale = "ru-RU";

        #endregion Private Fields

        #region Public Methods

        public static List<Screen> GetScreens()
        {
            List<Screen> screens = [];
            ushort i = 0;

            foreach (Display display in Display.GetDisplays())
            {
                PathDisplayTarget pDisplay = display.ToPathDisplayTarget();
                DisplaySetting dSettings = display.CurrentSetting;

                string name = FirstNotEmptyString(
                    [pDisplay.FriendlyName, display.DeviceName, display.DisplayFullName],
                    string.Empty
                );

                screens.Add(new Screen()
                {
                    Id = display.DisplayFullName,
                    Index = i++,
                    Name = name,
                    IsPrimary = display.IsGDIPrimary /*false*/,
                    IsConnected = true,
                    Bounds = new System.Drawing.Rectangle(dSettings.Position, dSettings.Resolution)
                });
            }

            return screens;
        }

        /// <summary>
        /// Load settings from file
        /// </summary>
        public void Load()
        {
            if (TryLoadData(out LocalSettings settings))
            {
                Copy(settings, this, true);
            }
            // Set screen indexes
            ushort i = 0;
            foreach (Screen screen in Screens) { screen.Index = i++; }
        }

        public List<Screen> ScreensUpdate()
        {
            List<Screen> screens = GetScreens();
            List<Screen> oldScreens = Screens;

            // Update already existsing screens in config
            foreach (Screen screen in screens)
            {
                // Find new screen in old screens list
                if (oldScreens.FirstOrDefault(old => old == screen) is Screen old)
                {
                    // Update new screen config
                    screen.Wallpaper = old.Wallpaper;
                    // And remove old from old screens list
                    oldScreens.Remove(old);
                }
            }

            // Update disconnected screens and add them to a new list
            foreach (Screen disconnected in oldScreens)
            {
                disconnected.Index = (ushort)screens.Count;
                disconnected.IsConnected = false;
                screens.Add(disconnected);
            }

            Screens = screens;
            return screens;
        }

        #endregion Public Methods

        #region Private Methods

        private static string FirstNotEmptyString(string[] arr, string defaultString)
        {
            foreach (string s in arr)
            {
                if (string.IsNullOrWhiteSpace(s)) { continue; }
                return s;
            }
            return defaultString;
        }

        #endregion Private Methods
    }
}
