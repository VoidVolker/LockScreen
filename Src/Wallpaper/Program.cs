using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Lib.DataTypes.Enums;
using Lib.Tools;

using LockScreen.DataTypes.Enums;

using Microsoft.Win32;

using Wallpaper.Tools;

using static Lib.Tools.Native;

using LSScreen = Lib.DataTypes.Structures.Screen;

namespace Wallpaper
{
    internal static class Program
    {
        private const string LogonServiceProcessName = "LogonService";

        /// <summary>
        /// Settings
        /// </summary>
        public static LocalSettings Settings;

        /// <summary>
        /// Application mode
        /// </summary>
        public static AppMode Mode =
            ParentProcessUtilities.GetParentProcess().ProcessName == LogonServiceProcessName
            ? AppMode.Logon
            : AppMode.User;

        /// <summary>
        /// Main window
        /// </summary>
        public static WallpaperForm MainForm;

        public static List<WallpaperForm> WallpapersForms = [];

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Init();
            Parse(args);
        }

        private static void AppRun(WallpaperForm main)
        {
            // Processing display settings changes
            SystemEvents.DisplaySettingsChanged += DisplaySettingsChanged;
            // IMPORTANT! Removing event listener on app exit is required
            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.systemevents.displaysettingschanged?view=windowsdesktop-9.0&redirectedfrom=MSDN
            main.Closing += MainWindow_Closing;

            Application.Run(MainForm);
        }

        private static void DisplaySettingsChanged(object sender, EventArgs e)
        {
            Settings.ScreensUpdate();
            foreach (var form in WallpapersForms)
            {
                if (Settings.Screens.FirstOrDefault(s => s.Id == form.ScreenId && s.IsConnected) is LSScreen screen)
                {
                    form.RepostionTo(screen.Bounds);
                    continue;
                }
                form.Close();
            }
        }

        private static void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            SystemEvents.DisplaySettingsChanged -= DisplaySettingsChanged;
        }

        public static void Parse(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                RunEmptyArgs();
            }
            else if (args.Length == 1)
            {
                // Allow to use slash instead backslash in display Id for simple using in command line
                string screenId = args[0].Replace('/', '\\');
                if (Settings.Screens.FirstOrDefault(s => s.Id == screenId) is LSScreen screen)
                {
                    RunFindScreen(screen);
                }
                else
                {
                    MsgBoxError($"Display not found: {screenId}");
                }
            }
            else if (args.Length == 2)
            {
                // Allow to use slash instead backslash in display Id for simple using in command line
                string screenId = args[0].Replace('/', '\\');
                string file = args[1];

                if (!File.Exists(file))
                {
                    MsgBoxError($"File not found: {file}");
                    return;
                }

                if (Settings.Screens.FirstOrDefault(s => s.Id == screenId) is LSScreen screen)
                {
                    RunOnScreen(screen, file);
                }
                else
                {
                    MsgBoxError($"Display not found: {screenId}");
                }
            }
            else if (args.Length == 5)
            {
                string file = args[4];

                if (!int.TryParse(args[0], out int x)) { MessageBox.Show($"Can't parse X position from string: {args[0]}"); }
                if (!int.TryParse(args[1], out int y)) { MessageBox.Show($"Can't parse Y position from string: {args[1]}"); }
                if (!uint.TryParse(args[2], out uint w)) { MessageBox.Show($"Can't parse Width from string: {args[2]}"); }
                if (!uint.TryParse(args[3], out uint h)) { MessageBox.Show($"Can't parse Heigth from string: {args[3]}"); }
                if (!File.Exists(file))
                {
                    MsgBoxError($"File not found: {file}");
                    return;
                }

                RunAtBounds(x, y, (int)w, (int)h, file);
            }
            else
            {
                MsgBoxError($"Wrong arguments count: {args.Length}");
            }
        }

        private static void MsgBoxError(string msg) =>
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static void Init()
        {
            ApplicationConfiguration.Initialize();
            Settings = new LocalSettings(Info.WorkFile(LocalSettings.FileName));
            Settings.Load();
            Settings.ScreensUpdate();
        }

        private static void RunFindScreen(LSScreen screen)
        {
            MainForm = new WallpaperForm(Mode, screen);
            WallpapersForms.Add(MainForm);
            AppRun(MainForm);
        }

        private static void RunOnScreen(LSScreen screen, string wallappaer)
        {
            MainForm = new WallpaperForm(Mode, screen.Id, wallappaer, screen.Bounds);
            WallpapersForms.Add(MainForm);
            AppRun(MainForm);
        }

        private static void RunAtBounds(int x, int y, int w, int h, string file)
        {
            Rectangle bounds = new(x, y, w, h);
            string screenId = string.Empty;
            if (Settings.Screens.FirstOrDefault(s => s.Bounds.Contains(x, y)) is LSScreen screen)
            {
                screenId = screen.Id;
            }

            MainForm = new WallpaperForm(Mode, screenId, file, bounds);
            WallpapersForms.Add(MainForm);
            AppRun(MainForm);
        }

        private static void RunEmptyArgs()
        {
            if (Settings.WallpaperMode == WallpaperMode.One)
            {
                if (!string.IsNullOrEmpty(Settings.Wallpaper) && !File.Exists(Settings.Wallpaper))
                {
                    return;
                }

                foreach (LSScreen screen in Settings.Screens)
                {
                    if (screen.IsPrimary) { continue; }
                    screen.Wallpaper = Settings.Wallpaper;
                    WallpapersForms.Add(new WallpaperForm(Mode, screen.Id, screen.Wallpaper, screen.Bounds));
                }
            }
            else
            {
                //string notFoundErr = "Wallpaper file not found";
                foreach (LSScreen screen in Settings.Screens)
                {
                    if (string.IsNullOrEmpty(screen.Wallpaper)
                        || !File.Exists(screen.Wallpaper)
                        || screen.IsPrimary)
                    {
                        continue;
                    }

                    WallpapersForms.Add(new WallpaperForm(Mode, screen.Id, screen.Wallpaper, screen.Bounds));
                }
            }

            if (WallpapersForms.Count == 0) { return; }

            MainForm = WallpapersForms.First();
            AppRun(MainForm);
        }
    }
}
