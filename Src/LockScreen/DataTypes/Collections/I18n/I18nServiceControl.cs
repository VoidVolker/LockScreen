using LockScreen.DataTypes.Enums;
using LockScreen.DataTypes.Interfaces;

namespace LockScreen.DataTypes.Collections.I18n
{
    public class I18nServiceControl : I18nEnum<I18nServiceControl, ServiceState>
    {
        #region Public Methods

        /// <summary>
        /// Implicit сonvert XEnumServiceControl to ServiceState
        /// </summary>
        /// <param name="item"></param>
        public static implicit operator ServiceState(I18nServiceControl item) => item.Enum;

        /// <summary>
        /// Implicit сonvert ServiceState to XEnumServiceControl
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator I18nServiceControl(ServiceState type) => Find(type);

        #endregion Public Methods
    }
}
