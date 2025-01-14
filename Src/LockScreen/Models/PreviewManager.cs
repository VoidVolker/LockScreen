using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Lib.Tools;

using LockScreen.Tools;

namespace LockScreen.Models
{
    /// <summary>
    /// Preview management
    /// </summary>
    public class PreviewManager
    {
        #region Public Constructors

        public PreviewManager()
        {
        }

        #endregion Public Constructors

        #region Public Fields

        /// <summary>
        /// List of running preview apps
        /// </summary>
        public readonly ConcurrentDictionary<string, WallpaperApp> PreviewApps = [];

        #endregion Public Fields

        #region Private Fields

        // Doesn't have any real sense, but just for case
        private bool IsFindStarting = false;

        private WallpaperApp ScreenFindApp = null;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Preview window closed event
        /// </summary>
        public event EventHandler<WallpaperApp> Closed;

        /// <summary>
        /// Preview window opened event
        /// </summary>
        public event EventHandler<WallpaperApp> Opened;

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// Close preview window for screen
        /// </summary>
        /// <param name="screenId">Screen system ID</param>
        public void Close(string screenId)
        {
            if (PreviewApps.Remove(screenId, out WallpaperApp app))
            {
                app.Kill();
                app.WaitForExit();
            }
        }

        /// <summary>
        /// Close all wallpaper preview windows
        /// </summary>
        public void CloseAll()
        {
            PreviewApps.Each((id, app) =>
            {
                app.Kill();
                app.WaitForExit();
            });
            PreviewApps.Clear();
        }

        public void FindApp(string screenId)
        {
            IsFindStarting = true;
            if (ScreenFindApp != null)
            {
                ScreenFindApp.Kill();
                if (ScreenFindApp.ScreenId == screenId)
                {
                    return;
                }
            }
            ScreenFindApp = new WallpaperApp(screenId.Replace('\\', '/'));
            ScreenFindApp.Exited += ScreenFindAppExited;
            IsFindStarting = false;
        }

        /// <summary>
        /// Check if screen has opened preview window
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public bool IsOpened(string screenId) => PreviewApps.ContainsKey(screenId);

        /// <summary>
        /// Open preview window for screen
        /// </summary>
        /// <param name="screenId">Screen system ID</param>
        /// <param name="wallpaper">Wallpaper image file</param>
        public void Open(string screenId, string wallpaper)
        {
            if (PreviewApps.TryGetValue(screenId, out WallpaperApp runninApp))
            {
                runninApp.Kill();
                runninApp.WaitForExit();
            }
            WallpaperApp app = new(screenId, wallpaper);
            PreviewApps[screenId] = app;
            app.Exited += Proc_Exited;
            Opened?.Invoke(this, app);
        }

        #endregion Public Methods

        #region Private Methods

        private void Proc_Exited(object sender, EventArgs e)
        {
            WallpaperApp app = sender as WallpaperApp;
            app.Exited -= Proc_Exited;
            Closed?.Invoke(this, app);
        }

        private void ScreenFindAppExited(object sender, EventArgs e)
        {
            if (sender is WallpaperApp app && ReferenceEquals(app, ScreenFindApp) && !IsFindStarting)
            {
                app.Exited -= ScreenFindAppExited;
                ScreenFindApp = null;
            }
        }

        #endregion Private Methods
    }
}
