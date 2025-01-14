using System;

namespace LockScreen.Exceptions
{
    public class ElementNotFoundException(Type type, string name)
        : I18nException(
            "Element not found",
            [type.FullName, name]
        )
    {
        public readonly string ElementName = name;
        public readonly Type ElementType = type;
    }
}
