using Lib.DataTypes.Enums;

using LockScreen.DataTypes.Interfaces;

namespace LockScreen.DataTypes.Collections.I18n
{
    public class I18nWallpaperMode : I18nEnum<I18nWallpaperMode, WallpaperMode>
    {
        #region Public Methods

        /// <summary>
        /// Implicit сonvert XEnumWallpaperMode to WallpaperMode
        /// </summary>
        /// <param name="item"></param>
        public static implicit operator WallpaperMode(I18nWallpaperMode item) => item.Enum;

        /// <summary>
        /// Implicit сonvert WallpaperMode to XEnumWallpaperMode
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator I18nWallpaperMode(WallpaperMode type) => Find(type);

        #endregion Public Methods
    }
}
