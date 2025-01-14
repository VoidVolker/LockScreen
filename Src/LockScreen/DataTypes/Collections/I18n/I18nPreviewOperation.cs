using LockScreen.DataTypes.Interfaces;

namespace LockScreen.DataTypes.Collections.I18n
{
    public class I18nPreviewOperation : I18nEnum<I18nPreviewOperation, bool>
    {
        #region Public Methods

        public static implicit operator bool(I18nPreviewOperation item) => item.Enum;

        public static implicit operator I18nPreviewOperation(bool type) => Find(type);

        #endregion Public Methods
    }
}
