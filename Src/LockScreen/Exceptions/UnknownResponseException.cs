using System;

namespace LockScreen.Exceptions
{
    public class UnknownResponseException(string response) : I18nException("Unknown response", [response])
    {
        public string Response = response;
    }
}
