using LockScreen.DataTypes.Enums;
using LockScreen.DataTypes.Interfaces;

namespace LockScreen.DataTypes.Collections.I18n
{
    public class I18nServiceState : I18nEnum<I18nServiceState, ServiceState>
    {
        #region Public Methods

        /// <summary>
        /// Implicit сonvert XEnumServiceState to ServiceState
        /// </summary>
        /// <param name="item"></param>
        public static implicit operator ServiceState(I18nServiceState item) => item.Enum;

        /// <summary>
        /// Implicit сonvert ServiceState to XEnumServiceState
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator I18nServiceState(ServiceState type) => Find(type);

        #endregion Public Methods
    }
}
