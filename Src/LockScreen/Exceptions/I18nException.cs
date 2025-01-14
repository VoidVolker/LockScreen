using System;

namespace LockScreen.Exceptions
{
    /// <summary>
    /// Localized exception
    /// </summary>
    /// <param name="stringId">i18n string Id</param>
    /// <param name="args">String arguments for format</param>
    public class I18nException(string stringId, object[] args)
        : Exception(
            string.Format(
                I18n($"{ExceptionId} {stringId}"),
                args))
    {
        public readonly object[] Args = args;
        public readonly string StringId = stringId;
        private const string ExceptionId = "Exception";
    }
}
