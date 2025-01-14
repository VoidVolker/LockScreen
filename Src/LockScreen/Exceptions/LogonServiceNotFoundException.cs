namespace LockScreen.Exceptions
{
    public class LogonServiceNotFoundException(string path) : I18nException("Logon service not found", [path])
    {
        public string Path = path;
    }
}
